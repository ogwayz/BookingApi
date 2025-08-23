namespace BookingApi.Domain
{
    public class BookingDto
    {
        public int roomId { get; set; }
        public DateTime startDate { get; set; }
        public DateTime endDate { get; set; }
    }
}