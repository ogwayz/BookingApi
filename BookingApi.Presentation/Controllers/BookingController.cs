using Microsoft.AspNetCore.Mvc;
using BookingApi.Aplication.Services;
using BookingApi.Domain;
using Microsoft.AspNetCore.Authorization;
using System.Globalization;
using System.Security.Claims;

namespace BookingApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetBooking(int id)
        {

            var booking = _bookingService.GetBooking(id);
            return booking != null ? Ok(booking) : NotFound();
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult AddBooking([FromBody] BookingDto bookingDto)
        {

            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID not found in token");
            }
            var room = _bookingService.GetRoom(bookingDto.roomId);
            if (room == null)
            {
                return NotFound("Room not found");
            }
            var booking = new Booking
            {
                Room = room,
                UserId = userId,
                StartDate = bookingDto.startDate,
                EndDate = bookingDto.endDate
            };
            try
            {
                _bookingService.AddBooking(booking);
                return CreatedAtAction(nameof(GetBooking), new { id = booking.Id }, booking);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
        }

    }

}