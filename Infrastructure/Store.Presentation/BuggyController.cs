using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class BuggyController : ControllerBase
    {
        [HttpGet("notfound")] // GET: baseUrl/api/buggy/notfound
        public IActionResult GetNotFoundRequest()
        {
            // logic
            return NotFound();
        }

        [HttpGet("badrequest")] // GET: baseUrl/api/buggy/badrequest
        public IActionResult GetBadRequest()
        {
            // logic
            return BadRequest();
        }

        [HttpGet("servererror")] // GET: baseUrl/api/buggy/servererror
        public IActionResult GetServerErrorRequest()
        {
            // logic
            throw new Exception();
            return Ok();
        }

        [HttpGet("badrequest/{id}")] // GET: baseUrl/api/buggy/badrequest/id
        public IActionResult GetNotFoundRequest(int id)
        {
            // logic
            return BadRequest();
        }

        [HttpGet("unauthorized")] // GET: baseUrl/api/buggy/unauthorized
        public IActionResult GetUnAuthorizedRequest()
        {
            // logic
            return Unauthorized();
        }
    }
}
