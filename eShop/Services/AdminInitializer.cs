using System.Threading.Tasks;
using eShop.Models;
using Microsoft.AspNetCore.Identity;

namespace eShop.Services
{
    public class AdminInitializer
    {
        public static async Task SeedAdminUser(
            RoleManager<IdentityRole> roleManager,
            UserManager<User> userManager)
        {
            const string adminEmail = "admin@admin.com";
            const string adminPassword = "123456789";
            const string name = "Admin";

            const string role = "admin";
            if (await roleManager.FindByNameAsync(role) is null)
            {
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            
            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User {Email = adminEmail, UserName = adminEmail, Name = name};
                var result = await userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}