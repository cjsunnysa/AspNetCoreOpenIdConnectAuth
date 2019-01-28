using CoreSecurityPrototype.Authentication;
using CoreSecurityPrototype.Data;
using CoreSecurityPrototype.Data.Models;
using CoreSecurityPrototype.Migrations;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CoreSecurityPrototype
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("CoreAuthPrototype");

            services.AddDbContext<AuthPrototypeContext>(options => options.UseSqlServer(connectionString));
            services
                .AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<AuthPrototypeContext>();

            services.AddScoped<AuthorizationProvider>();
            
            services
                .AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddOAuthValidation(OpenIdConnectDefaults.AuthenticationScheme)
                .AddOpenIdConnectServer(options =>
                {
                    options.TokenEndpointPath = "/connect/token";
                    options.AccessTokenLifetime = TimeSpan.FromHours(1);
                    options.RefreshTokenLifetime = null;
                    options.ProviderType = typeof(AuthorizationProvider);
                });

            services
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(
            IApplicationBuilder app,
            IHostingEnvironment env,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            IdentityDataInitializer.SeedData(userManager, roleManager);

            app.UseAuthentication();
            app.UseMvc();
        }
    }
}
