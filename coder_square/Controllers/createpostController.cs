using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;
using System.Security.Claims;

namespace coder_square.Controllers
{
    
    [ApiController]
    [Authorize]
    public class createpostController : ControllerBase
    {
        public readonly codersquareContext db = new codersquareContext();

        [HttpPost, Route("/posts")]
        public async Task<IActionResult> CraetePost(createpost createpost)
        {
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var new_post = new Post();
            new_post.Title = createpost.title;
            new_post.Url=createpost.url;
            new_post.Date = DateTime.Now;
            new_post.Likes = 0;
            new_post.NumComments = 0;
            new_post.UserId = userId;
            await db.Posts.AddAsync(new_post);
            await db.SaveChangesAsync();
            return Ok(new_post);
        }

    }
}
