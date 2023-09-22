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
            var rooms = _context.Rooms
                .Where(room => room.HotelId == HotelId)
                .Select(room => new RoomDto
                {
                    RoomId = room.RoomId,
                    Name = room.Name,
                    Capacity = room.Capacity,
                    Image = room.Image,
                    Hotel = new HotelDto
                    {
                        HotelId = room.Hotel!.HotelId,
                        Name = room.Hotel.Name,
                        Address = room.Hotel.Address,
                        CityId = room.Hotel.CityId,
                        CityName = room.Hotel.City!.Name
                    }
                })
                .ToList();

            return rooms;
        }

        // 8. Refatore o endpoint POST /room
        public RoomDto AddRoom(Room room) {
            _context.Rooms.Add(room);
            _context.SaveChanges();

            return new RoomDto
            {
                RoomId = room.RoomId,
                Name = room.Name,
                Capacity = room.Capacity,
                Image = room.Image,
                Hotel = _context.Hotels
                .Where(hotel => hotel.HotelId == room.HotelId)
                .Select(h => new HotelDto
                {
                    HotelId = h.HotelId,
                    Name = h.Name,
                    Address = h.Address,
                    CityId = h.CityId,
                    CityName = _context.Cities
                    .Where(city => city.CityId == h.CityId)
                    .Select(c => c.Name)
                    .FirstOrDefault()
                })
                .FirstOrDefault()
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