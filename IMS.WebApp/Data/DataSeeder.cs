using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace IMS.WebApp.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(UserManager<IdentityUser> userManager)
    {
        await SeedUserAsync(userManager, "admin@ims.com", "Administration");
        await SeedUserAsync(userManager, "inventory@ims.com", "InventoryManagement");
        await SeedUserAsync(userManager, "sales@ims.com", "Sales");
        await SeedUserAsync(userManager, "purchasing@ims.com", "Purchasing");
        await SeedUserAsync(userManager, "production@ims.com", "ProductionManagement");
    }

    private static async Task SeedUserAsync(UserManager<IdentityUser> userManager, string email, string department)
    {
        var user = await userManager.FindByEmailAsync(email);
        if (user == null)
        {
            user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, "P@ssword123");
            if (result.Succeeded)
            {
                await userManager.AddClaimAsync(user, new Claim("Department", department));
            }
        }
    }
}
