﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using coder_square.Helper;
using coder_square.Models;

namespace coder_square.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private codersquareContext db;
        public ValuesController(codersquareContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public async Task<IActionResult> getall()
        {
            var all_p= db.Posts.ToList();
            return Ok(all_p);

        }

        //////////////testing some singe ///////////////////
        
        [HttpGet , Route("/Posts/{p_id}/lists/{l_id}")]
        //[Route("/Posts/{p_id}/lists/{l_id}")]
        public async Task<IActionResult> get_by_id(int p_id, int l_id ,int m ,string ff , viewposts v)
        {
               return Ok(p_id);
        }
        [HttpPost, Route("/Posts_post")]
        //[Route("/Posts/{p_id}/lists/{l_id}")]
        public async Task<IActionResult> Posts_post(viewposts v ,int id )
        {
            return Ok(id);
        }

    }
}
