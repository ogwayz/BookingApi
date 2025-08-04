using System.Data.Common;
using System.Runtime.CompilerServices;
using BookingApi.Domain;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BookingApi.Aplication.Services
{

    public class BookingService : IBookingService
    {

        public readonly BookingDbContext _context;

        public BookingService(BookingDbContext context)
        {
            _context = context;
        }

        public Room GetRoom(int id)
        {
            return _context.Rooms.FirstOrDefault(r => r.Id == id);
        }

        public void AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            _context.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            var room = _context.Rooms.Find(id);
            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }
        
        public void AddBooking(Booking booking)
        {

            if (booking.StartDate >= booking.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate");
            }

            if (!IsBookingAvailable(booking.Room, booking.StartDate, booking.EndDate))
            {
                throw new InvalidOperationException("Room is already booked for this period");
            }
            _context.Bookings.Add(booking);
            _context.SaveChanges();
        }
        public Booking GetBooking(int id)
        {
            return _context.Bookings.FirstOrDefault(b => b.Id == id);
        }

        public bool IsBookingAvailable(Room Room, DateTime StartDate, DateTime EndDate)
        {

            
            return !_context.Bookings.Any(b => 
                b.Room.Id == Room.Id && 
                StartDate < b.EndDate && 
                EndDate > b.StartDate);
        }
    }
}