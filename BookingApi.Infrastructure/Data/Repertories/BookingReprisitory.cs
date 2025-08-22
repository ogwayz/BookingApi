using BookingApi.Domain;
using Microsoft.EntityFrameworkCore;
using BookingApi.Infrastructure.Data;

public class BookingReprisitory
{
    private readonly BookingDbContext _context;

    public BookingReprisitory(BookingDbContext context)
    {
        _context = context;
    }

    public Booking? GetById(int id)
    {
        return _context.Bookings.FirstOrDefault(b => b.Id == id);
    }

    public void Add(Booking booking)
    {
        _context.Bookings.Add(booking);
        _context.SaveChanges();
    }

    public bool IsAvaliable(Room room, DateTime startDate, DateTime endDate)
    {
        return !_context.Bookings.Any(b =>
                b.Room.Id == room.Id &&
                startDate < b.EndDate &&
                endDate > b.StartDate);
    }
}