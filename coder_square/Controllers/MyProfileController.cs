using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace coder_square.Controllers
{
    
    [ApiController]
    [Authorize]
    public class MyProfileController : ControllerBase
    {
        private codersquareContext db;
        public MyProfileController(codersquareContext db)
        {
            this.db = db;
        }
        //--------view profile for  the user that  has been login  HIS NAME AND ALL HIS POSTS-----------\\
        [HttpGet, Route("/MyProfile")]
        public async Task<IActionResult> View_Profile()
        {
            var id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var check_user = db.AspNetUsers.Where(x => x.Id == id).Select(x =>
                  new
                  {
                      x.Id
                  }
            ).FirstOrDefault();
            if (check_user is null)
            {
                return NotFound();
            }

            var target_user = new MyProfile();

            // ADD His POSTS 
            target_user.his_posts = (from u in db.AspNetUsers
                                     join p in db.Posts on u.Id equals p.UserId

                                     where p.UserId == id

                                     select new viewposts
                                     {
                                         Likes = p.Likes,
                                         Date = p.Date,
                                         user_id = p.UserId,
                                         user_name = u.UserName,
                                         Url = p.Url,
                                         Title = p.Title,
                                         post_id = p.Id,
                                         number_comments = p.NumComments

                                     }).OrderByDescending(x => x.Date).ToList();


            // ADD NAME
            var user_name = db.AspNetUsers.Where(x => x.Id == id).Select(x =>
                  new
                  {
                      x.UserName
                  }
            ).FirstOrDefault();

            target_user.name = user_name.UserName;

            //add user_id
            target_user.user_id = id;

            return Ok(target_user);
        }


        //------------------------------------UPDATE HIS  NAME ------------------------------------\\
        [HttpPut, Route("/MyProfile/{user_id}/MyName")]
        public async Task<IActionResult> Update_Name(string user_id, string name)
        {
            var chesk_user = db.AspNetUsers.Where(x => x.Id == user_id).Select(x =>
                  new
                  {
                      x.Id
                  }
            ).FirstOrDefault();

            var userIdLogin = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userIdLogin != user_id || user_id is null || chesk_user is null)
            {
                return NotFound();
            }

            var target_user = db.AspNetUsers.Where(x => x.Id == userIdLogin).FirstOrDefault();
            target_user.UserName = name;

            db.AspNetUsers.Update(target_user);
            db.SaveChanges();

            return Ok();
        }


        //----------------------------------DELETE  POST ---------------------------------\\
        [HttpDelete, Route("/MyProfile/Posts/{post_id}")]
        public async Task<IActionResult> Delete_post(int post_id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var target_post = db.Posts.Where(x => x.Id == post_id).FirstOrDefault();

            if(target_post is null || userId!=target_post.UserId)
            {
                return NotFound();
            }

            // Delete all  related likes for this post 
            var RelatedLikes = db.Likes.Where(x => x.PostId == post_id).ToList();
            db.Likes.RemoveRange(RelatedLikes);


            // Delete all  related comments for this post 
            var RelatedComments = db.Comments.Where(x => x.PostId == post_id).ToList();
            db.Comments.RemoveRange(RelatedComments);

            // Delete all  related Saved Details  for this post 
            var Related_SavedDetails = db.SavedDetails.Where(x => x.PostId == post_id).ToList();
            db.SavedDetails.RemoveRange(Related_SavedDetails);

            //finaly delete our target post 
            db.Posts.Remove(target_post);
            db.SaveChanges();

            return NoContent();
        }


        //----------------------------------UPDATE  POST ---------------------------------\\
        [HttpPut, Route("/MyProfile/Posts/{post_id}")]
        public async Task<IActionResult> Update_Post(int post_id, createpost post)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var target_post = db.Posts.Where(x => x.Id == post_id).FirstOrDefault();

            if (target_post is null || userId != target_post.UserId)
            {
                return NotFound();
            }

            target_post.Url = post.url;
            target_post.Title = post.title;

            db.Posts.Update(target_post);
            db.SaveChanges();

            return Ok();
        }


    }


}
