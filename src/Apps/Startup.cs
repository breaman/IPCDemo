using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Apps
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(authorizationOptions =>
            {
                authorizationOptions.AddPolicy("CanAccessMyIPC",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("myipc");
                        policyBuilder.RequireAssertion(context => context.User.HasClaim(c => c.Type == "myipc" && c.Value.ToLower() != "declined"));
                    });

                authorizationOptions.AddPolicy("CanUseMyIPC",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("myipc", "approved");
                    });

                authorizationOptions.AddPolicy("CanAccessGIS",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("gis");
                        policyBuilder.RequireAssertion(context => context.User.HasClaim(c => c.Type == "gis" && c.Value.ToLower() != "declined"));
                    });

                authorizationOptions.AddPolicy("CanUseGIS",
                    policyBuilder =>
                    {
                        policyBuilder.RequireAuthenticatedUser();
                        policyBuilder.RequireClaim("gis", "approved");
                    });
            });

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
                options.Scope.Add("roles");
                options.Scope.Add("myipc");
                options.Scope.Add("gis");

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    NameClaimType = JwtClaimTypes.Name,
                    RoleClaimType = JwtClaimTypes.Role
                };

                // options.Events.OnUserInformationReceived = (context) => { System.Console.WriteLine(context.User); return Task.CompletedTask; };

                options.ClaimActions.MapUniqueJsonKey(JwtClaimTypes.WebSite, JwtClaimTypes.WebSite);
                options.ClaimActions.MapJsonKey(JwtClaimTypes.Role, JwtClaimTypes.Role);
                options.ClaimActions.MapUniqueJsonKey("myipc", "myipc");
                options.ClaimActions.MapUniqueJsonKey("gis", "gis");

                options.Events.OnRedirectToIdentityProvider = context => {
                    string requestPath = context.Request.Path.Value?.ToLower();

                    if (!string.IsNullOrWhiteSpace(requestPath))
                    {
                        if (requestPath.IndexOf("myipc") > -1)
                        {
                            context.ProtocolMessage.AcrValues = "tenant:myipc";
                        }
                        else if (requestPath.IndexOf("gis") > -1)
                        {
                            context.ProtocolMessage.AcrValues = "tenant:gis";
                        }
                    }
                    return Task.FromResult(0);
                };
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
