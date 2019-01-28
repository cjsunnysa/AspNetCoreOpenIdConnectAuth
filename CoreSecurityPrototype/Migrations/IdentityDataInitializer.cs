using CoreSecurityPrototype.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace CoreSecurityPrototype.Migrations
{
    public class IdentityDataInitializer
    {
        public static void SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync("Administrator").Result)
            {
                var role = new IdentityRole { Name = "Administrator" };
                var result = roleManager.CreateAsync(role).Result;
            }

            if (userManager.FindByNameAsync("admin").Result == null)
            {
                var user = new ApplicationUser { UserName = "admin", Email = "admin@it-s.co.za" };
                var result = userManager.CreateAsync(user, "Password123!").Result;

                if (result.Succeeded)
                    userManager.AddToRoleAsync(user, "Administrator").Wait();
            }
        }
    }
}
