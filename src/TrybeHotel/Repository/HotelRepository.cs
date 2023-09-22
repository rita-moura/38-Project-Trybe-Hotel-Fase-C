using TrybeHotel.Models;
using TrybeHotel.Dto;
using Microsoft.EntityFrameworkCore;

namespace TrybeHotel.Repository
{
    public class HotelRepository : IHotelRepository
    {
        protected readonly ITrybeHotelContext _context;
        public HotelRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        //  5. Refatore o endpoint GET /hotel
        public IEnumerable<HotelDto> GetHotels()
        {
            var hotels = _context.Hotels
            .Include(h => h.City)
            .Select(hotel => new HotelDto
            {
                HotelId = hotel.HotelId,
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId,
                CityName = hotel.City!.Name
            })
            .ToList();

            return hotels;
        }

        // 6. Refatore o endpoint POST /hotel
        public HotelDto AddHotel(Hotel hotel)
        {
           var city = _context.Cities.FirstOrDefault(city => city.CityId == hotel.CityId);

            var newHotel = new Hotel
            {
                Name = hotel.Name,
                Address = hotel.Address,
                CityId = hotel.CityId
            };

            _context.Hotels.Add(newHotel);
            _context.SaveChanges();

            return new HotelDto
            {
                HotelId = newHotel.HotelId,
                Name = newHotel.Name,
                Address = newHotel.Address,
                CityId = newHotel.CityId,
                CityName = city?.Name?? ""
            };
        }
    }
}