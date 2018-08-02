using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Transistor.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public IActionResult AdminAccess()
        {
            return new JsonResult(new[] { "You have admin access" });
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public IActionResult UserAccess()
        {
            return new JsonResult(new [] { "You have user access" });
        }
    }
}
