using Microsoft.EntityFrameworkCore;
using BookingApi.Domain;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace BookingApi.Infrastructure.Data
{

    public class BookingDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        
        public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
        {
        }

        /*
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=localhost;Port=5433;Database=BookingDb;User ID=admin;Password=admin123;");
        }
        */

    }

}