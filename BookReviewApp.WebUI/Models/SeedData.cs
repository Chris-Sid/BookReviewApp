using BookReviewApp.DataAccess;
using BookReviewApp.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookReviewApp.WebUI.Models
{
    public static class SeedData
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string[] roles = { "Admin", "Customer" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_SEED_PASSWORD") ?? "Admin2025!";
            var customerPassword = Environment.GetEnvironmentVariable("CUSTOMER_SEED_PASSWORD") ?? "Customer2025!";
            await CreateUserIfNotExists(userManager, "Admin", "admin@example.com", adminPassword, "Admin");
            await CreateUserIfNotExists(userManager, "Customer", "customer@example.com", customerPassword, "Customer");
        }

        private static async Task CreateUserIfNotExists(
            UserManager<AppUser> userManager,
            string username,
            string email,
            string password,
            string role)
        {
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                user = new AppUser { UserName = username, Email = email, EmailConfirmed = true };
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, role);
            }
        }
    }
}
