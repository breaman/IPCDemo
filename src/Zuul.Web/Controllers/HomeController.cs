using System.Diagnostics;
using System.Threading.Tasks;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Mvc;
using Zuul.Web.Attributes;
using Zuul.Web.ViewModels;

namespace Zuul.Web.Controllers
{
    [SecurityHeaders]
    public class HomeController : Controller
    {
        private IIdentityServerInteractionService IdentityServerInteractionService { get; }

        public HomeController(IIdentityServerInteractionService interaction)
        {
            IdentityServerInteractionService = interaction;
        }

        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Shows the error page
        /// </summary>
        public async Task<IActionResult> Error(string errorId)
        {
            var vm = new ErrorViewModel();

            // retrieve error details from identityserver
            var message = await IdentityServerInteractionService.GetErrorContextAsync(errorId);
            if (message != null)
            {
                vm.Error = message;
            }

            return View("Error", vm);
        }
    }
}
