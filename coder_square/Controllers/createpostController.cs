using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using System.Security.Claims;

namespace coder_square.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class createpostController : ControllerBase
    {
        public readonly codersquareContext db = new codersquareContext();

        [HttpPost, Route("/createpost/create")]
        public async Task<IActionResult> create(createpost createpost)
        {
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var new_post = new Post();
            new_post.Title = createpost.title;
            new_post.Url=createpost.url;
            new_post.Date = DateTime.Now;
            new_post.Likes = 0;
            new_post.NumComments = 0;
            new_post.UserId = userId;
            db.Posts.Add(new_post);
            db.SaveChanges();
            return Ok(new_post);
        }
    }
}
