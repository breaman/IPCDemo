using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Zuul.Domain.Models;
using Zuul.Web.Services;
using Zuul.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zuul.Web.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private UserManager<User> UserManager { get; }
        private SignInManager<User> SignInManager { get; }
        private ILogger<ManageController> Logger { get; }
        private UrlEncoder UrlEncoder { get; }
        private IEmailSender EmailSender { get; }
        private ISmsSender SmsSender { get; }

        private const string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
        private const string RecoveryCodesKey = nameof(RecoveryCodesKey);

        public ManageController(
            UserManager<User> userManager,
            SignInManager<User> signInManager,
            ILogger<ManageController> logger,
            UrlEncoder urlEncoder,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            Logger = logger;
            UrlEncoder = urlEncoder;
            EmailSender = emailSender;
            SmsSender = smsSender;
        }

        // GET: /<controller>/
        public async Task<IActionResult> Index(ManageMessageId? message = null)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }

            var viewModel = new IndexViewModel
            {
                HasPassword = await UserManager.HasPasswordAsync(user),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(user),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(user),
                BrowserRemembered = await SignInManager.IsTwoFactorClientRememberedAsync(user)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await SmsSender.SendSmsAsync(model.PhoneNumber, "Your security code is: " + code);
            return RedirectToAction(nameof(VerifyPhoneNumber), new { PhoneNumber = model.PhoneNumber });
        }

        [HttpGet]
        public async Task<IActionResult> EnableTwoFactorAuth(string returnUrl = null)
        {
            var viewModel = new EnableTwoFactorAuthViewModel();
            var user = await GetCurrentUserAsync();

            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }
            var userFactors = new List<string> {"", "Authenticator", "Phone" };  // await UserManager.GetValidTwoFactorProvidersAsync(user);

            viewModel.Providers = userFactors.Select(purpose => new SelectListItem { Text = purpose, Value = purpose }).ToList();
            viewModel.ReturnUrl = returnUrl;

            await LoadSharedKeyAndQrCodeUriAsync(user, viewModel);

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EnableTwoFactorAuth(EnableTwoFactorAuthViewModel viewModel)
        {
            var user = await GetCurrentUserAsync();
            IActionResult actionResult = View(viewModel);

            switch(viewModel.SelectedProvider.ToLower())
            {
                case "phone":
                    // need to send verification code and then redirect to the verify page
                    TempData["ReturnUrl"] = viewModel.ReturnUrl;
                    actionResult = RedirectToAction(nameof(VerifyPhoneNumber), "Manage", new { phoneNumber = viewModel.PhoneNumber });
                    break;
                case "authenticator":
                    actionResult = await EnableAuthenticatorAuth(viewModel);
                    break;
            }

            return actionResult;
        }

        private async Task<IActionResult> EnableAuthenticatorAuth(EnableTwoFactorAuthViewModel model)
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            // Strip spaces and hypens
            var verificationCode = model.AuthenticatorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(
                user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

            if (!is2faTokenValid)
            {
                ModelState.AddModelError("Code", "Verification code is invalid.");
                await LoadSharedKeyAndQrCodeUriAsync(user, model);
                return View(model);
            }

            await UserManager.SetTwoFactorEnabledAsync(user, true);
            Logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
            var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
            TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

            return RedirectToAction(nameof(ShowRecoveryCodes));
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EnableTwoFactorAuthentication()
        //{
        //    var user = await GetCurrentUserAsync();

        //    if (user != null)
        //    {
        //        await UserManager.SetTwoFactorEnabledAsync(user, true);
        //        await SignInManager.SignInAsync(user, isPersistent: false);
        //        Logger.LogInformation(1, "User enabled two-factor authentication");
        //    }

        //    return RedirectToAction(nameof(Index), "Manage");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await UserManager.SetTwoFactorEnabledAsync(user, false);
                await SignInManager.SignInAsync(user, isPersistent: false);
                Logger.LogInformation(2, "User disabled two-factor authentication.");
            }
            return RedirectToAction(nameof(Index), "Manage");
        }

        [HttpGet]
        public async Task<IActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var returnUrl = TempData["ReturnUrl"] as string;
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return View("Error");
            }
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(user, phoneNumber);
            // Send an SMS to verify the phone number
            await SmsSender.SendSmsAsync(phoneNumber, $"Your verification code is {code}");
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber, ReturnUrl = returnUrl });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await UserManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                if (result.Succeeded)
                {
                    await UserManager.SetTwoFactorEnabledAsync(user, true);
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index));
                }
            }
            // If we got this far, something failed, redisplay the form
            ModelState.AddModelError(string.Empty, "Failed to verify phone number");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await UserManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction(nameof(Index), new { Message = ManageMessageId.RemovePhoneSuccess });
                }
            }
            return RedirectToAction(nameof(Index), new { Message = ManageMessageId.Error });
        }

        [HttpGet]
        public async Task<IActionResult> TwoFactorAuthentication()
        {
            var user = await UserManager.GetUserAsync(User);
            if (user == null)
            {
                throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'");
            }

            var viewModel = new TwoFactorAuthenticationViewModel
            {
                HasAuthenticator = await UserManager.GetAuthenticatorKeyAsync(user) != null,
                Is2faEnabled = user.TwoFactorEnabled,
                RecoveryCodesLeft = await UserManager.CountRecoveryCodesAsync(user),
            };

            return View(viewModel);
        }

        //[HttpGet]
        //public async Task<IActionResult> EnableAuthenticator()
        //{
        //    var user = await UserManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
        //    }

        //    var model = new EnableAuthenticatorViewModel();
        //    await LoadSharedKeyAndQrCodeUriAsync(user, model);

        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EnableAuthenticator(EnableAuthenticatorViewModel model)
        //{
        //    var user = await UserManager.GetUserAsync(User);
        //    if (user == null)
        //    {
        //        throw new ApplicationException($"Unable to load user with ID '{UserManager.GetUserId(User)}'.");
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        await LoadSharedKeyAndQrCodeUriAsync(user, model);
        //        return View(model);
        //    }

        //    // Strip spaces and hypens
        //    var verificationCode = model.Code.Replace(" ", string.Empty).Replace("-", string.Empty);

        //    var is2faTokenValid = await UserManager.VerifyTwoFactorTokenAsync(
        //        user, UserManager.Options.Tokens.AuthenticatorTokenProvider, verificationCode);

        //    if (!is2faTokenValid)
        //    {
        //        ModelState.AddModelError("Code", "Verification code is invalid.");
        //        await LoadSharedKeyAndQrCodeUriAsync(user, model);
        //        return View(model);
        //    }

        //    await UserManager.SetTwoFactorEnabledAsync(user, true);
        //    Logger.LogInformation("User with ID {UserId} has enabled 2FA with an authenticator app.", user.Id);
        //    var recoveryCodes = await UserManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 10);
        //    TempData[RecoveryCodesKey] = recoveryCodes.ToArray();

        //    return RedirectToAction(nameof(ShowRecoveryCodes));
        //}

        [HttpGet]
        public IActionResult ShowRecoveryCodes()
        {
            var recoveryCodes = (string[])TempData[RecoveryCodesKey];
            if (recoveryCodes == null)
            {
                return RedirectToAction(nameof(TwoFactorAuthentication));
            }

            var model = new ShowRecoveryCodesViewModel { RecoveryCodes = recoveryCodes };
            return View(model);
        }

        #region Helpers
        private async Task LoadSharedKeyAndQrCodeUriAsync(User user, EnableTwoFactorAuthViewModel model)
        {
            var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await UserManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
            }

            model.SharedKey = FormatKey(unformattedKey);
            model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                UrlEncoder.Encode("IPCDemo"), // need a better way of getting this based on what app is doing the request in case you have multiple apps accessing the same ID server
                UrlEncoder.Encode(email),
                unformattedKey);
        }

        private string FormatKey(string unformattedKey)
        {
            var result = new StringBuilder();
            int currentPosition = 0;
            while (currentPosition + 4 < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                currentPosition += 4;
            }
            if (currentPosition < unformattedKey.Length)
            {
                result.Append(unformattedKey.Substring(currentPosition));
            }

            return result.ToString().ToLowerInvariant();
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            AddLoginSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        private Task<User> GetCurrentUserAsync()
        {
            return UserManager.GetUserAsync(User);
        }

        #endregion
    }
}
