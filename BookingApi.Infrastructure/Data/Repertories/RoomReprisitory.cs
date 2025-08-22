using BookingApi.Domain;
using Microsoft.EntityFrameworkCore;
using BookingApi.Infrastructure.Data;

public class RoomReprisitory
{
    private readonly BookingDbContext _context;

    public RoomReprisitory(BookingDbContext context)
    {
        _context = context;
    }

    public Room? GetById(int id)
    {
        return _context.Rooms.FirstOrDefault(r => r.Id == id);
    }

    public void Add(Room room)
    {
        _context.Rooms.Add(room);
    }

    public void Delete(Room room)
    {
        _context.Rooms.Remove(room);
        _context.SaveChanges();
    }
}