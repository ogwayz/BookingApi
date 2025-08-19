namespace BookingApi.Domain
{
    public class Room
    {
        public int Id { get; set; }
        public required string Class { get; set; }
        public int Price { get; set; }
        public string? Description { get; set; }
        public bool IsAvailable { get; set; }
    }
}