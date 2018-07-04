using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace WebApi1
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(options => options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie()
                .AddOpenIdConnect(options =>
                {
                    options.ClientId = "12943214587-05o071imcpvc6v47pdmejdltqpagntrj.apps.googleusercontent.com";
                    options.ClientSecret = "zeFwmUGznrVA9XfO28_8-6Xt";
                    options.Authority = "https://accounts.google.com";
                    options.ResponseType = OpenIdConnectResponseType.Code;
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                    options.Events = new OpenIdConnectEvents()
                    {
                        OnRedirectToIdentityProvider = (context) =>
                        {
                            if (context.Request.Path != "/account/external")
                            {
                                context.Response.Redirect("/account/login");
                                context.HandleResponse();
                            }

                            return Task.FromResult(0);
                        }
                    };
                });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
