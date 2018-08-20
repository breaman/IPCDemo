using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Apps.Controllers
{
    [Authorize]
    public class MyIPCController : Controller
    {
        [Authorize(Policy ="CanAccessMyIPC")]
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [Authorize(Policy = "CanUseMyIPC")]
        public IActionResult DoSomething()
        {
            return View();
        }
    }
}
