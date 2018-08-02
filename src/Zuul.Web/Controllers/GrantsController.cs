using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Zuul.Web.Attributes;
using Zuul.Web.ViewModels;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Zuul.Web.Controllers
{
    /// <summary>
    /// This sample controller allows a user to revoke grants given to clients
    /// </summary>
    [SecurityHeaders]
    [Authorize]
    public class GrantsController : Controller
    {
        private IIdentityServerInteractionService IdentityServerInteractionService { get; }
        private IClientStore ClientStore { get; }
        private IResourceStore ResourceStore { get; }

        public GrantsController(IIdentityServerInteractionService interaction,
            IClientStore clients,
            IResourceStore resources)
        {
            IdentityServerInteractionService = interaction;
            ClientStore = clients;
            ResourceStore = resources;
        }

        /// <summary>
        /// Show list of grants
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View("Index", await BuildViewModelAsync());
        }

        /// <summary>
        /// Handle postback to revoke a client
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Revoke(string clientId)
        {
            await IdentityServerInteractionService.RevokeUserConsentAsync(clientId);
            return RedirectToAction("Index");
        }

        private async Task<GrantsViewModel> BuildViewModelAsync()
        {
            var grants = await IdentityServerInteractionService.GetAllUserConsentsAsync();

            var list = new List<GrantViewModel>();
            foreach (var grant in grants)
            {
                var client = await ClientStore.FindClientByIdAsync(grant.ClientId);
                if (client != null)
                {
                    var resources = await ResourceStore.FindResourcesByScopeAsync(grant.Scopes);

                    var item = new GrantViewModel()
                    {
                        ClientId = client.ClientId,
                        ClientName = client.ClientName ?? client.ClientId,
                        ClientLogoUrl = client.LogoUri,
                        ClientUrl = client.ClientUri,
                        Created = grant.CreationTime,
                        Expires = grant.Expiration,
                        IdentityGrantNames = resources.IdentityResources.Select(x => x.DisplayName ?? x.Name).ToArray(),
                        ApiGrantNames = resources.ApiResources.Select(x => x.DisplayName ?? x.Name).ToArray()
                    };

                    list.Add(item);
                }
            }

            return new GrantsViewModel
            {
                Grants = list
            };
        }
    }
}
