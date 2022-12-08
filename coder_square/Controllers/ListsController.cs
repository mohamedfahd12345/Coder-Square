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
    public class ListsController : ControllerBase
    {
        private codersquareContext db;
        public ListsController(codersquareContext db)
        {
            this.db = db;
        }
        //-------------------TO VIEW  LISTS FOR USER THAT  HAS BEEN LOGIN ------------------\\
        [HttpGet, Route("/Lists")]
        public async Task<IActionResult> Get_My_Lists()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var All_Lists = db.Saveds.Where(x=>x.UserId==userId).Select(x =>
                 
                 new Lists
                 {
                     List_Name = x.SavedName ,
                     List_Id   = x.SavedId
                 }
            ).ToList();

            return Ok(All_Lists);
        }


        [HttpPost, Route("/Lists")]
        public async Task<IActionResult> Create_List(Lists lists)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var new_list = new Saved();
            new_list.SavedName = lists.List_Name;
            new_list.UserId = userId;

            db.Saveds.Add(new_list);
            db.SaveChanges();


            return Ok();
        }


        [HttpDelete, Route("/Lists/{id}")]
        public async Task<IActionResult> Delete_List(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Target_List = db.Saveds.Where(x => x.SavedId == id).FirstOrDefault();

            if(Target_List is null || Target_List.UserId != userId)
            {
                return BadRequest();
            }

            // Delete related saved details 
            var Related_List_Details = db.SavedDetails.Where(x => x.SavedId == id).ToList();
            db.SavedDetails.RemoveRange(Related_List_Details);
            

            // Delete Target list 
            db.Saveds.Remove(Target_List);

            db.SaveChanges();

            return NoContent();
        }


        [HttpPut, Route("/Lists/{id}")]
        public async Task<IActionResult> Update_List_Name (int id , Lists lists)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var Target_List = db.Saveds.Where(x => x.SavedId == id).FirstOrDefault();

            if (Target_List is null || Target_List.UserId != userId || id != lists.List_Id)
            {
                return NotFound();
            }

            Target_List.SavedName = lists.List_Name;
            db.Saveds.Update(Target_List);
        
            db.SaveChanges();

            return Ok();
        }

    }
}
