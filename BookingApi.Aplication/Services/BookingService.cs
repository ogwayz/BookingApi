using System.Data.Common;
using System.Runtime.CompilerServices;
using BookingApi.Domain;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BookingApi.Aplication.Services
{
    public class BookingService : IBookingService
    {
        private readonly RoomReprisitory _roomReprisitory;
        private readonly BookingReprisitory _bookingReprisitory;

        public BookingService(RoomReprisitory roomReprisitory, BookingReprisitory bookingReprisitory)
        {
            _roomReprisitory = roomReprisitory;
            _bookingReprisitory = bookingReprisitory;
        }

        public Room GetRoom(int id)
        {
            return _roomReprisitory.GetById(id);
        }
        public void AddRoom(Room room)
        {
            _roomReprisitory.Add(room);
        }

        public void DeleteRoom(int id)
        {
            var room = _roomReprisitory.GetById(id);
            if (room != null)
            {
                _roomReprisitory.Delete(room);
            }
        }

        public void AddBooking(Booking booking)
        {

            if (booking.StartDate >= booking.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate");
            }

            if (!_bookingReprisitory.IsAvaliable(booking.Room, booking.StartDate, booking.EndDate))
            {
                throw new InvalidOperationException("Room is already booked for this period");
            }
            _bookingReprisitory.Add(booking);
        }
        public Booking GetBooking(int id)
        {
            return _bookingReprisitory.GetById(id);
        }
    }
}