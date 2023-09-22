using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : Controller
    {
        private readonly IRoomRepository _repository;
        public RoomController(IRoomRepository repository)
        {
            _repository = repository;
        }

        [HttpGet("{HotelId}")]
        public IActionResult GetRoom(int HotelId){
            try
            {
                return Ok(_repository.GetRooms(HotelId));
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost]
        [Authorize(Policy = "Admin")]
        public IActionResult PostRoom([FromBody] Room room){
            try
            {
                return Created("", _repository.AddRoom(room));
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpDelete("{RoomId}")]
        [Authorize(Policy = "Admin")]
        public IActionResult Delete(int RoomId)
        {
            try
            {
                _repository.DeleteRoom(RoomId);
                return NoContent();
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}