using System;
using System.Threading.Tasks;
using BookingApi.Domain;
using BookingApi.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookingApi.Aplication.Services
{
    public class SeedService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SeedService(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            
            var roles = new[] { "Admin", "User" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            var adminEmail = Environment.GetEnvironmentVariable("ADMIN_EMAIL");
            var adminPassword = Environment.GetEnvironmentVariable("ADMIN_PASSWORD");

            if (string.IsNullOrEmpty(adminEmail) || string.IsNullOrEmpty(adminPassword))
            {
                throw new InvalidOperationException("ADMIN_EMAIL or ADMIN_PASSWORD is not set in .env file.");
            }

            Console.WriteLine($"Seeding admin user with email: {adminEmail}");
            var admin = await _userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                    Console.WriteLine($"Admin user {adminEmail} created and assigned to Admin role.");
                }
                else
                {
                    throw new InvalidOperationException($"Failed to create admin user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
            else
            {
                Console.WriteLine($"Admin user {adminEmail} already exists.");
            }
        }
    }
}