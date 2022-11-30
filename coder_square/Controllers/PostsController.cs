using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Models;
using coder_square.Helper;

namespace coder_square.Controllers
{
  
    [ApiController]
    public class PostsController : ControllerBase
    {
        
        public readonly codersquareContext db = new codersquareContext();


        //----------------------------TO VIEW THE LASTEST POSTS--------------------------------\\
        [HttpGet ,Route("/latest-Posts") ]
        public async Task<IActionResult> ViewLatesPosts()
        {
            var all_posts = (from u in db.AspNetUsers
                             join p in db.Posts on u.Id equals p.UserId

                            
                             select new viewposts
                             {
                                 Likes = p.Likes,
                                 Date = p.Date,
                                 user_id = p.UserId,
                                 user_name = u.UserName,
                                 Url = p.Url,
                                 Title = p.Title,
                                 post_id=p.Id,
                                 number_comments = p.NumComments

                             }).OrderByDescending(x => x.Date).ToList();
            return Ok(all_posts);
        }


        //--------------------------TO VIEW THE MOST LIKES POSTS--------------------------------\\
        [HttpGet, Route("/most-likes-posts")]
        public async Task<IActionResult> ViewMostLikesPosts()
        {
            var all_posts = (from u in db.AspNetUsers
                             join p in db.Posts on u.Id equals p.UserId


                             select new viewposts
                             {
                                 Likes = p.Likes,
                                 Date = p.Date,
                                 user_id = p.UserId,
                                 user_name = u.UserName,
                                 Url = p.Url,
                                 Title = p.Title,
                                 post_id = p.Id,
                                 number_comments=p.NumComments

                             }).OrderByDescending(x => x.Likes).ToList();

            return Ok(all_posts);
        }

      

    }
}
