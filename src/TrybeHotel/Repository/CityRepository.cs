using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class CityRepository : ICityRepository
    {
        protected readonly ITrybeHotelContext _context;
        public CityRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 4. Refatore o endpoint GET /city
        public IEnumerable<CityDto> GetCities()
        {
            var cities = _context.Cities
                .Select(city => new CityDto
                {
                    CityId = city.CityId,
                    Name = city.Name,
                    State = city.State
                })
                .ToList();

            return cities;
        }

        // 2. Refatore o endpoint POST /city
        public CityDto AddCity(City city)
        {
            _context.Cities.Add(city);
            _context.SaveChanges();

            var newCity = new CityDto
            {
                CityId = city.CityId,
                Name = city.Name,
                State = city.State
            };

            return newCity;
        }

        // 3. Desenvolva o endpoint PUT /city
        public CityDto UpdateCity(City city)
        {
           var updatedCity = _context.Cities.FirstOrDefault(c => c.CityId == city.CityId);

           if(updatedCity == null)
           {
                return null!;
           }

           updatedCity.Name = city.Name;
           updatedCity.State = city.State;

           _context.SaveChanges();

           return new CityDto
           {
                CityId = city.CityId,
                Name = updatedCity.Name,
                State = updatedCity.State
           };
        }

    }
}