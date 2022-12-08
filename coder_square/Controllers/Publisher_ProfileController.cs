using coder_square.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;

namespace coder_square.Controllers
{
  
    [ApiController]
    public class Publisher_ProfileController : ControllerBase
    {

        private codersquareContext db;
        public Publisher_ProfileController(codersquareContext db)
        {
            this.db = db;
        }
        //--------------- VIEW Publisher_Profile HIS NAME AND ALL HIS POSTS-----------\\
        [HttpGet, Route("/Publisher-Profile/{Publisher_id}")]
        public async Task<IActionResult> ViewPublisherProfile(string Publisher_id)
        {
            var check_user = db.AspNetUsers.Where(x => x.Id == Publisher_id).Select(x =>
                  new
                  {
                      x.Id
                  }
            ).FirstOrDefault();

            if(check_user is null)
            {
                return NotFound();
            }

            var target_user = new Publisher_Profile();

            // ADD his  POSTS
            target_user.his_posts = (from u in db.AspNetUsers
                             join p in db.Posts on u.Id equals p.UserId

                             where p.UserId == Publisher_id

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
            var user_name = db.AspNetUsers.Where(x => x.Id == Publisher_id ).Select(x =>
                  new
                  {
                      x.UserName
                  }
            ).FirstOrDefault();

            target_user.name = user_name.UserName;

            // add publisher id 
            target_user.user_id = Publisher_id ;
            
            return Ok(target_user);
        }


    }
}
