using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zuul.Domain.Models;
using Zuul.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zuul.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private RoleManager<Role> RoleManager { get; }
        private UserManager<User> UserManager { get; }
        public AdminController(RoleManager<Role> roleManager, UserManager<User> userManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        #region User Management
        public async Task<IActionResult> Users()
        {
            UserAdminViewModel viewModel = new UserAdminViewModel();

            viewModel.Users = UserManager.Users.Select(u => new UserViewModel { Id = u.Id, FirstName = u.FirstName, LastName = u.LastName, Email = u.Email, EmailConfirmed = u.EmailConfirmed }).ToList();
            foreach (var user in viewModel.Users)
            {
                user.Roles = await UserManager.GetRolesAsync(await (UserManager.FindByIdAsync(user.Id.ToString())));
            }

            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> EnableUser(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var confirmationToken = await UserManager.GenerateEmailConfirmationTokenAsync(user);

                var result = await UserManager.ConfirmEmailAsync(user, confirmationToken);

                if (result.Succeeded)
                {
                    return RedirectToAction("Users");
                }
                else
                {
                    ViewBag.Message = "Unable to activate the user with the following reasons";
                    return View("UnableToUpdateUser", result.Errors);
                }
            }
            else
            {
                return View("UnableToFindUser", userId);
            }
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var viewModel = new UserViewModel { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName, Email = user.Email };

                viewModel.Roles = await UserManager.GetRolesAsync(user);

                return View(viewModel);
            }
            else
            {
                return View("UnableToFindUser", userId);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(UserViewModel viewModel)
        {
            var user = await UserManager.FindByIdAsync(viewModel.Id.ToString());
            if (user != null)
            {
                // check to see if the email address changed
                if (string.Compare(user.Email, viewModel.Email, true) != 0)
                {
                    var changeEmailToken = await UserManager.GenerateChangeEmailTokenAsync(user, viewModel.Email);
                    var emailChangeResult = await UserManager.ChangeEmailAsync(user, viewModel.Email, changeEmailToken);
                    
                    if (!emailChangeResult.Succeeded)
                    {
                        ViewBag.Message = "Unable to change the user's email address with the following reasons";
                        return View("UnableToUpdateUser", emailChangeResult.Errors);
                    }
                    var usernameChangeResult = await UserManager.SetUserNameAsync(user, viewModel.Email);
                }

                user.FirstName = viewModel.FirstName;
                user.LastName = viewModel.LastName;
                var updateResult = await UserManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    ViewBag.Message = "Unable to update the user with the following reasons";
                    return View("UnableToUpdateUser", updateResult.Errors);
                }

                // need to save off any role changes as well
            }
            else
            {
                return View("UnableToFindUser", viewModel.Id);
            }

            return RedirectToAction("Users");
        }

        [HttpGet]
        public async Task<IActionResult> SetPassword(int userId)
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            if (user != null)
            {
                var viewModel = new SetPasswordViewModel { Id = user.Id, FirstName = user.FirstName, LastName = user.LastName };

                return View(viewModel);
            }
            else
            {
                return View("UnableToFindUser", userId);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetPassword(SetPasswordViewModel viewModel)
        {
            var user = await UserManager.FindByIdAsync(viewModel.Id.ToString());
            if (user != null)
            {
                PasswordHasher<User> passwordHasher = new PasswordHasher<User>();

                string hashedPassword = passwordHasher.HashPassword(user, "helloworld");
                //var passwordResetToken = await UserManager.GeneratePasswordResetTokenAsync(user);
                //var passwordResetResponse = await UserManager.ResetPasswordAsync(user, passwordResetToken, viewModel.Password);
                //if (!passwordResetResponse.Succeeded)
                //{
                //    ViewBag.Message = "Unable to set the user's password with the following reasons";
                //    return View("UnableToUpdateUser", passwordResetResponse.Errors);
                //}
                return RedirectToAction("Users");
            }
            else
            {
                return View("UnableToFindUser", viewModel.Id);
            }
        }
        #endregion

        #region Role Management
        public IActionResult Roles()
        {
            RoleAdminViewModel viewModel = new RoleAdminViewModel();

            viewModel.Roles = RoleManager.Roles.Select(r => new RoleViewModel { Id = r.Id, Name = r.Name }).ToList();

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> AddRole(string roleName)
        {
            var roleCreationResult = await RoleManager.CreateAsync(new Role { Name = roleName });

            if (!roleCreationResult.Succeeded)
            {
                ViewBag.Message = $"Unable to create the role named {roleName} due to the following errors:";
                return View("UnableToUpdateRole", roleCreationResult.Errors);
            }

            return RedirectToAction("Roles");
        }
        #endregion
    }
}
