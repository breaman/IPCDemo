using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fuse.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fuse.Controllers
{
    public class MyIpcController : Controller
    {
        private ApplicationDbContext DbContext { get; }
        public MyIpcController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            var userIdsWithGisClaim = DbContext.UserClaims.Where(uc => uc.ClaimType == "myipc").Select(uc => uc.UserId).ToList();
            var users = DbContext.Users.Where(u => userIdsWithGisClaim.Contains(u.Id)).Include(u => u.Claims).ToList();
            return View();
        }
    }
}
