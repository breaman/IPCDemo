using System.IdentityModel.Tokens.Jwt;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Fuse
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "Cookies";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("Cookies")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "Cookies";

                options.Authority = "https://localhost:44324";
                options.RequireHttpsMetadata = true;

                options.SaveTokens = true;

                //options.ClientId = "mvc";
                //options.ResponseType = "id_token";

                options.ClientId = "mvc-hybrid";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";

                options.GetClaimsFromUserInfoEndpoint = true;

                options.Scope.Add("api1");
                options.Scope.Add("offline_access");

                // options.Events.OnUserInformationReceived = (context) => { System.Console.WriteLine(context.User); return Task.CompletedTask; };

                // options.ClaimActions.Add(new JsonKeyClaimAction("role", "role", "role")); // just something to keep track of in case we want roles returned in ID_TOKEN

                options.ClaimActions.MapJsonKey(JwtClaimTypes.WebSite, JwtClaimTypes.WebSite);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
