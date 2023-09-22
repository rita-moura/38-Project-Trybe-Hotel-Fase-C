using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;
using TrybeHotel.Dto;
using TrybeHotel.Services;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("login")]

    public class LoginController : Controller
    {

        private readonly IUserRepository _repository;
        public LoginController(IUserRepository repository)
        {
            _repository = repository;
        }

        [HttpPost]
        public IActionResult Login([FromBody] LoginDto login){
           try
            {   
                var userLogin = _repository.Login(login);

                if (userLogin == null)
                {
                    return StatusCode(401, new { message = "Incorrect e-mail or password" });
                }

                var token = new TokenGenerator().Generate(userLogin);
                
                return StatusCode(200, new { token });
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
    }
}