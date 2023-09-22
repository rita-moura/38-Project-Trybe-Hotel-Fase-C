using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using TrybeHotel.Dto;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("booking")]
  
    public class BookingController : Controller
    {
        private readonly IBookingRepository _repository;
        public BookingController(IBookingRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult Add([FromBody] BookingDtoInsert bookingInsert){
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(claims => claims.Type == ClaimTypes.Email)?.Value;
                var addBooking = _repository.Add(bookingInsert, email!);

                if (addBooking != null)
                {
                    return Created("", addBooking);
                }
                else
                {
                    return BadRequest(new { message = "Guest quantity over room capacity" });
                }
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            } 
        }


        [HttpGet("{Bookingid}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Policy = "Client")]
        public IActionResult GetBooking(int Bookingid){
            try
            {
                var token = HttpContext.User.Identity as ClaimsIdentity;
                var email = token?.Claims.FirstOrDefault(claims => claims.Type == ClaimTypes.Email)?.Value;
                var booking = _repository.GetBooking(Bookingid, email!);

                if (booking != null)
                {
                    return Ok(booking);
                }
                else
                {
                    return  Unauthorized();
                }
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            } 
        }
    }
}