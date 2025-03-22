using Bakalauras.Core.Entities;
using Bakalauras.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Bakalauras.Core.Services;

public static class AuthDbSeeder
{
    public static async Task SeedDataAsync(ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
    {
        // Seed roles if they don't exist
        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole("Admin"));
        }
        if (!await roleManager.RoleExistsAsync("Customer"))
        {
            await roleManager.CreateAsync(new IdentityRole("Customer"));
        }
        if (!await roleManager.RoleExistsAsync("Mechanic"))
        {
            await roleManager.CreateAsync(new IdentityRole("Mechanic"));
        }

        // Seed admin user if it doesn't exist
        var adminEmail = "example@email.com";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new User
            {
                UserName = adminEmail,
                Email = adminEmail,
                FirstName = "Admin",
                LastName = "User",
                PhoneNumber = "+1234567890",
                RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7)
            };

            var result = await userManager.CreateAsync(adminUser, "Password1234@");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
