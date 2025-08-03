using Microsoft.AspNetCore.Mvc;
using BookingApi.Aplication.Services;
using BookingApi.Domain;
using Microsoft.AspNetCore.Authorization;

namespace BookingApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public RoomController(IBookingService bookingService)
        {
            _bookingService = bookingService ?? throw new ArgumentNullException(nameof(bookingService));
        }

        [HttpGet("{id}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public IActionResult GetRoom(int id)
        {
            System.Console.WriteLine($"[DEBUG] GetRoom called with id: {id} User: {User?.Identity?.Name} IsAuthenticated: {User?.Identity?.IsAuthenticated}");
            var room = _bookingService.GetRoom(id);
            return room != null ? Ok(room) : NotFound("Room with id " + id.ToString() + " not found");
        }

        [HttpPost]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public IActionResult AddRoom([FromBody] Room room)
        {
            if (room == null) return BadRequest("Room data is required");
            _bookingService.AddRoom(room);
            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin", AuthenticationSchemes = "Bearer")]
        public IActionResult DeleteRoom(int id)
        {
            _bookingService.DeleteRoom(id);
            return NoContent();
        }
    }

}