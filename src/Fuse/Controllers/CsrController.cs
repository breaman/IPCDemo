using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Fuse.Domain.Models;
using Fuse.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fuse.Controllers
{
    public class CsrController : Controller
    {
        private ApplicationDbContext DbContext { get; }
        public CsrController(ApplicationDbContext dbContext)
        {
            DbContext = dbContext;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            var viewModel = new SetPasswordViewModel { UserId = id };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(SetPasswordViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var user = DbContext.Users.FirstOrDefault(u => u.Id == viewModel.UserId);
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                user.PasswordHash = hasher.HashPassword(user, viewModel.Password);

                var rowsUpdated = DbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
    }
}
