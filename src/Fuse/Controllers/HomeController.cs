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
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> CallApiUsingUserAccessToken()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.SetBearerToken(accessToken);
            try
            {
                var result = await client.GetAsync("https://localhost:44363/api/identity");
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
