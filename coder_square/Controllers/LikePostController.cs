using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using System.Security.Claims;


namespace coder_square.Controllers
{
    
    [ApiController]
    public class LikePostController : ControllerBase
    {
        public readonly codersquareContext db = new codersquareContext();
        
        [HttpGet, Route("/LikePost/IsLiked")]
        public async Task<IActionResult> IsLiked(int post_id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); 
            var target_user=db.Likes.Where(x=>x.UserId == userId && x.PostId==post_id).FirstOrDefault();

            if(target_user is null)
            {
                return Ok("NO");
            }

            return Ok("YES");
        }
        //------------------------- WHEN USER CLICK ON LIKE BUTTON -------------------------------\\
        [Authorize]
        [HttpPost, Route("/LikePost/Like")]
        public async Task<IActionResult> Like(int post_id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var target_like = db.Likes.Where(x => x.UserId == userId && x.PostId == post_id).FirstOrDefault();

            var target_post = db.Posts.Where(x => x.Id == post_id).FirstOrDefault();

            if(target_post is null)
            {
                return NotFound();
            }
            
            if (target_like is null)
            {
                var new_like = new Like();
                new_like.PostId=post_id;
                new_like.UserId=userId;
                db.Likes.Add(new_like);

                target_post.Likes++;
                db.Posts.Update(target_post);

                db.SaveChanges();
                return Ok();
            }

            db.Likes.Remove(target_like);

            target_post.Likes--;
            db.Posts.Update(target_post);

            db.SaveChanges();
            return Ok();
        }
    }
}
