using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using System.Security.Claims;

namespace coder_square.Controllers
{
  
    [ApiController]
    public class commentsController : ControllerBase
    {
        public readonly codersquareContext db = new codersquareContext();
        
         //---------------------------CREATE COMMENTS-----------------------------------------\\ 
        [HttpPost, Route("/comments/create")]
        [Authorize]
        public async Task<IActionResult> create(createcomment createcomment)
        {
            var target_post = db.Posts.Where(x => x.Id == createcomment.post_id).Select(x=>
                new
                {
                    x.Id
                }
                ).FirstOrDefault();
            if (createcomment == null || target_post is null)
                return NotFound();
            

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var new_comment = new Comment();
            new_comment.Date= DateTime.Now;
            new_comment.UserId = userId;
            new_comment.PostId = createcomment.post_id;
            new_comment.Descripiton = createcomment.description;
            db.Comments.Add(new_comment);
            db.SaveChanges();


            //--------INCREMENT NUMBER OF COMMENTS FOR THIS POST IN TABLE POSTS---------\\
            var targetPOST = db.Posts.Where(x => x.Id == createcomment.post_id).FirstOrDefault();
            targetPOST.NumComments++;
            db.Posts.Update(targetPOST);
            db.SaveChanges();

            return Ok("DONE");
        }


        //------------------------VIEW ALL COMMENTS FOR SPECIFIC POST BY ID-------------------\\
        [HttpGet, Route("/comments/View_All")]
        public async Task<IActionResult> View_All(int id)
        {
            var target_post = db.Posts.Where(x => x.Id == id).Select(x=>
            new
            {
                x.Id
            }
            ).FirstOrDefault();

            if (target_post is null)
                return NotFound();


            //-------------------------VIEW ALL COMMENTS ---------------------------\\
            var view_Comments = new ViewComments();
            view_Comments.post_id = id;
           
             view_Comments.allcomments = (from c in db.Comments
                        join u in db.AspNetUsers on c.UserId equals u.Id

                        where c.PostId == id

                        select new Comments
                        {
                            user_name = u.UserName,
                            description = c.Descripiton,
                            Date = c.Date
                        }).ToList();
            

            return Ok(view_Comments);
        }


    }
}
