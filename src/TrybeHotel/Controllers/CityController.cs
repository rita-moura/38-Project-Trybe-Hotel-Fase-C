using Microsoft.AspNetCore.Mvc;
using TrybeHotel.Models;
using TrybeHotel.Repository;

namespace TrybeHotel.Controllers
{
    [ApiController]
    [Route("city")]
    public class CityController : Controller
    {
        private readonly ICityRepository _repository;
        public CityController(ICityRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult GetCities(){
           try
            {
               return Ok(_repository.GetCities());
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult PostCity([FromBody] City city){
             try
            {
                return Created("", _repository.AddCity(city));
            }
            catch (ApplicationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
        }
        
        // 3. Desenvolva o endpoint PUT /city
        [HttpPut]
        public IActionResult PutCity([FromBody] City city){
            throw new NotImplementedException();
        }
    }
}