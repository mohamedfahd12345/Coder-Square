using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using System.Security.Claims;

namespace coder_square.Controllers
{
    [Authorize]
    [ApiController]
    public class ListsDetailsController : ControllerBase
    {
        private codersquareContext db;
        public ListsDetailsController(codersquareContext db)
        {
            this.db = db;
        }
        //------------------------View all posts for this list -------------------------\\
        [HttpGet, Route("/ListsDetails/{List_Id}")]
        public async Task<IActionResult> GetListsDetails(int List_Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var TargetList = db.Saveds.Where(x => x.SavedId == List_Id).FirstOrDefault();


            if (TargetList is null || TargetList.UserId != userId)
            {
                return NotFound();
            }

            var ListsDetails = new Lists_Details();

            ListsDetails.List_Id = List_Id;
            
            ListsDetails.Posts = (from LD in db.SavedDetails
                                  join P in db.Posts
                                  on LD.PostId equals P.Id
                                  join u in db.AspNetUsers
                                  on P.UserId equals u.Id

                                  where LD.SavedId == List_Id

                                  select new viewposts
                                  {

                                      Likes = P.Likes,
                                      Date = P.Date,
                                      user_id = P.UserId,
                                      user_name = u.UserName,
                                      Url = P.Url,
                                      Title = P.Title,
                                      post_id = P.Id,
                                      number_comments = P.NumComments

                                  }

                                  ).ToList();


            
            return Ok(ListsDetails);

        }



        [HttpDelete, Route("/Lists/{List_Id}/posts/{Post_Id}")]
        public async Task<IActionResult> DeletePostFromList(int List_Id, int Post_Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var TargetList = db.Saveds.Where(x => x.SavedId == List_Id).FirstOrDefault();

            var target_post = db.SavedDetails.Where(x => x.PostId == Post_Id && x.SavedId == List_Id).FirstOrDefault();

            if (TargetList is null || TargetList.UserId != userId || target_post is null)
            {
                return NotFound();
            }

            try
            {
                db.SavedDetails.Remove(target_post);
                db.SaveChanges();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Ok();


        }




        [HttpPost , Route("/Lists/{List_Id}/posts/{Post_Id}")]
        public async Task<IActionResult> AddPostToList(int List_Id, int Post_Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var TargetList = db.Saveds.Where(x => x.SavedId == List_Id).Select(x =>
                new
                {
                    x.SavedId,
                    x.UserId
                }

            ).FirstOrDefault();


            var target_post = db.Posts.Where(x => x.Id == Post_Id).Select(x =>
                new
                {
                    x.Id
                }
            ).FirstOrDefault();


            if (TargetList is null || TargetList.UserId != userId || target_post is null)
            {
                return NotFound();
            }

            var CheckRowExist = db.SavedDetails.Where(x => x.SavedId == List_Id && x.PostId == Post_Id).FirstOrDefault();
            
            if(CheckRowExist is not null)
            {
                return Ok();
            }

            try
            {
                var NewSavedDetails = new SavedDetail();
                NewSavedDetails.PostId = Post_Id;
                NewSavedDetails.SavedId = List_Id;

                await db.SavedDetails.AddAsync(NewSavedDetails);

                await db.SaveChangesAsync();
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            return Ok();


        }





    }
}
