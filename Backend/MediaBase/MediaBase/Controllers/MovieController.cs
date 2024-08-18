﻿using Microsoft.AspNetCore.Mvc;
using MediaBase.Models;
using MediaBase.Services;

namespace MediaBase.Controllers
{
    public class MovieController : ControllerBase<Movie>
    {
        public MovieController(MovieRequestManager requestManager)
            : base(requestManager) { }

        [HttpGet("stream")]
        public IActionResult GetStream([FromQuery] string title)
        {
            try
            {
                var fileStreamResult = ((MovieRequestManager)requestManager).GetStream(title);
                return fileStreamResult;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}