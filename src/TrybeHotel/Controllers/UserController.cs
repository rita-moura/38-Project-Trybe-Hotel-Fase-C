using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("user")]

    public class UserController : Controller
    {
        private readonly IUserRepository _repository;
        public UserController(IUserRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        [Authorize(Policy = "Admin")]
        public IActionResult GetUsers()
        {
           try
            {
                var users = _repository.GetUsers();

                if (users != null) 
                {

                    return Ok(users);
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

        [HttpPost]
        public IActionResult Add([FromBody] UserDtoInsert user)
        {
            try
            {
                var newUser = _repository.Add(user);
                
                return CreatedAtAction("Add", new { userId = newUser.UserId }, newUser);
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}