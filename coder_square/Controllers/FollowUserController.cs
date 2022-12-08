using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using System.Security.Claims;


namespace coder_square.Controllers
{
    
    [ApiController]
    public class FollowUserController : ControllerBase
    {
        private codersquareContext db;
        public FollowUserController(codersquareContext db)
        {
            this.db = db;
        }
        [HttpGet, Route("/FollowUser/IsFollowing")]
        public async Task<IActionResult> IsFollowing(string father_id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var find = db.Followers.Where(x=>x.FatherId==father_id && x.ChildId==userId).FirstOrDefault();

            if (find is null)
            {
                return Ok("NO");
            }

            return Ok("YES");
        }

        //----------------------------- WHEN USER CLICK ON FOLLOW -----------------------------\\
        [Authorize]
        [HttpPost, Route("/FollowUser/Follow")]
        public async Task<IActionResult> Follow(string father_id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var find = db.Followers.Where(x => x.FatherId == father_id && x.ChildId == userId).FirstOrDefault();

            var target_father = db.AspNetUsers.Where(x => x.Id == father_id).FirstOrDefault();
            var target_child = db.AspNetUsers.Where(x => x.Id == userId).FirstOrDefault();

            if(target_child is null || target_father is null)
            {
                return NotFound();
            }

            
            if (find is null)
            {
                if(target_father.Followers is null)
                {
                    target_father.Followers = 0;
                }

                target_father.Followers++;
                db.AspNetUsers.Update(target_father);

                var new_follow = new Follower();
                new_follow.FatherId = father_id;
                new_follow.ChildId = userId;
                db.Followers.Add(new_follow);
                db.SaveChanges();
                return Ok();
            }

            target_father.Followers--;
            db.AspNetUsers.Update(target_father);

            db.Followers.Remove(find);
            db.SaveChanges();
            return Ok();
        }
    }
}
