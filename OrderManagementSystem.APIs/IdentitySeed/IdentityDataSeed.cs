using Microsoft.AspNetCore.Identity;
using OrderManagementSystem.Core.Entites.Identity;

namespace OrderManagementSystem.APIs.IdentitySeed
{
    public static class IdentityDataSeed
    {
        public static async Task SeedAsync(UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var adminRole = new IdentityRole("Admin");
            if (!await roleManager.RoleExistsAsync(adminRole.Name))
            {
                await roleManager.CreateAsync(adminRole);
            }

            var customerRole = new IdentityRole("Customer");
            if (!await roleManager.RoleExistsAsync(customerRole.Name))
            {
                await roleManager.CreateAsync(customerRole);
            }

            var adminUser = new AppUser
            {
                UserName = "admin",
                Email = "admin@yahoo.com",
                EmailConfirmed = true
            };

            if (await userManager.FindByEmailAsync(adminUser.Email) == null)
            {
                var result = await userManager.CreateAsync(adminUser, "Mazen123@");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole.Name);
                }
            }
        }
    }
}
