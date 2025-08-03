using System.Data.Common;
using System.Runtime.CompilerServices;
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
            var adminEmail = "admin@example.com";
            var admin = await _userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var result = await _userManager.CreateAsync(admin, "AdminPass123!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }

}