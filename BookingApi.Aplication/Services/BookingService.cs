using System.Data.Common;
using System.Runtime.CompilerServices;
using BookingApi.Domain;
using BookingApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;


namespace BookingApi.Aplication.Services
{
    public class BookingService : IBookingService
    {
        private readonly RoomRepository _roomRepository;
        private readonly BookingRepository _bookingRepository;

        public BookingService(RoomRepository roomRepository, BookingRepository bookingRepository)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
        }

        public Room GetRoom(int id)
        {
            return _roomRepository.GetById(id);
        }
        public void AddRoom(Room room)
        {
            _roomRepository.Add(room);
        }

        public void DeleteRoom(int id)
        {
            var room = _roomRepository.GetById(id);
            if (room != null)
            {
                _roomRepository.Delete(room);
            }
        }

        public void AddBooking(Booking booking)
        {

            if (booking.StartDate >= booking.EndDate)
            {
                throw new ArgumentException("StartDate must be before EndDate");
            }

            if (!_bookingRepository.IsAvaliable(booking.Room, booking.StartDate, booking.EndDate))
            {
                throw new InvalidOperationException("Room is already booked for this period");
            }
            _bookingRepository.Add(booking);
        }
        public Booking GetBooking(int id)
        {
            return _bookingRepository.GetById(id);
        }
    }
}