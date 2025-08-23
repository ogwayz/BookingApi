namespace BookingApi.Domain
{
    public class Booking
    {
        public int Id { get; set; }
        public required Room Room { get; set; }
        public required string UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}