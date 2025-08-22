using BookingApi.Domain;

namespace BookingApi.Aplication.Services
{
    public interface IBookingService
    {
        Room GetRoom(int id);
        void AddRoom(Room room);
        void DeleteRoom(int id);
        void AddBooking(Booking booking);
        Booking GetBooking(int id);
    }
}