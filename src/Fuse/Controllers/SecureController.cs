using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Fuse.Controllers
{
    [Authorize]
    public class SecureController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Admin()
        {
            return View();
        }

        [Authorize(Roles = "User")]
        public IActionResult User()
        {
            return View();
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            return await CallApi("");
        }

        public async Task<IActionResult> CallApiUsingUserAccessTokenRequiringAdminRole()
        {
            return await CallApi("admin");
        }

        public async Task<IActionResult> CallApiUsingUserAccessTokenRequiringUserRole()
        {
            return await CallApi("user");
        }

        private async Task<IActionResult> CallApi(string apiEndpoint)
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            try
            {
                var result = await client.GetAsync($"https://localhost:44363/api/identity/{apiEndpoint}");
                // var content = await client.GetStringAsync("https://localhost:44363/api/identity");

                if (result.IsSuccessStatusCode)
                {
                    ViewBag.Json = JArray.Parse(await result.Content.ReadAsStringAsync()).ToString();
                }
                else
                {
                    ViewBag.Json = $"Status Code: {result.StatusCode}";
                }
            }
            catch (HttpRequestException hre)
            {
                ViewBag.Json = $"HResult: {hre.HResult}, Message: { hre.Message }";
            }
            return View("json");
        }
    }
}