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
        public readonly codersquareContext db = new codersquareContext();

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



        [HttpDelete, Route("/ListsDetails/{id}")]
        public async Task<IActionResult> DeletePostFromListsDetails(int List_Id , int Post_Id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var TargetList = db.Saveds.Where(x => x.SavedId == List_Id).FirstOrDefault();

            var target_post = db.SavedDetails.Where(x => x.PostId == Post_Id && x.SavedId==List_Id).FirstOrDefault();

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


       






    }
}
