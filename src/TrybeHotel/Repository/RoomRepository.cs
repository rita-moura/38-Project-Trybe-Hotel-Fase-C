using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class RoomRepository : IRoomRepository
    {
        protected readonly ITrybeHotelContext _context;
        public RoomRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 7. Refatore o endpoint GET /room
        public IEnumerable<RoomDto> GetRooms(int HotelId)
        {
            var hotel = _context.Hotels.First(h => h.HotelId == HotelId);
            var city = _context.Cities.First(c => c.CityId == hotel.CityId);
            var rooms = _context.Rooms.Where(r => r.HotelId == HotelId).ToList();
            
            var roomDtos = rooms.Select(room => new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = city.CityId,
                    CityName = city.Name,
                    State = city.State
                }
            });

            return roomDtos;
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            var hotel = _context.Hotels.First(h => h.HotelId == room.HotelId);
            var city = _context.Cities.First(c => c.CityId == hotel.CityId);

            _context.Rooms.Add(room);
            _context.SaveChanges();

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = new HotelDto
                {
                    HotelId = hotel.HotelId,
                    Name = hotel.Name,
                    Address = hotel.Address,
                    CityId = hotel.CityId,
                    CityName = city.Name,
                    State = city.State
                }
            };
        }

        public void DeleteRoom(int RoomId) 
        {
            var deleteRoom = _context.Rooms.Find(RoomId);

            if (deleteRoom != null)
            {
                _context.Rooms.Remove(deleteRoom);
                _context.SaveChanges();
            };
        }
    }
}