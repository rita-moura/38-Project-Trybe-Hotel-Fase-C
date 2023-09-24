using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class BookingRepository : IBookingRepository
    {
        protected readonly ITrybeHotelContext _context;
        public BookingRepository(ITrybeHotelContext context)
        {
            _context = context;
        }

        // 9. Refatore o endpoint POST /booking
        public BookingResponse Add(BookingDtoInsert booking, string email)
        {
           var roomToBook = _context.Rooms.Find(booking.RoomId);

            var user = _context.Users.Where((u) => u.Email == email).FirstOrDefault();

            if (roomToBook!.Capacity >= booking.GuestQuant)
            {
                var bookingToSave = new Booking
                {
                    CheckIn = booking.CheckIn,
                    CheckOut = booking.CheckOut,
                    GuestQuant = booking.GuestQuant,
                    RoomId = booking.RoomId,
                    UserId = user!.UserId
                };

                _context.Bookings.Add(bookingToSave);
                _context.SaveChanges();

                var response = from book in _context.Bookings
                              orderby book.BookingId
                              select new BookingResponse
                              {
                                  BookingId = book.BookingId,
                                  CheckIn = book.CheckIn,
                                  CheckOut = book.CheckOut,
                                  GuestQuant = book.GuestQuant,
                                  Room = new RoomDto
                                  {
                                      RoomId = book.Room!.RoomId,
                                      Name = book.Room.Name,
                                      Capacity = book.Room.Capacity,
                                      Image = book.Room.Image,
                                      Hotel = new HotelDto
                                      {
                                          HotelId = book.Room.Hotel!.HotelId,
                                          Name = book.Room.Hotel.Name,
                                          Address = book.Room.Hotel.Address,
                                          CityId = book.Room.Hotel.CityId,
                                          CityName = book.Room.Hotel.City!.Name,
                                          State = book.Room.Hotel.City.State
                                      }
                                  }
                              };
                return response.Last();
            }
            else
            {
                return null!;
            }
        }

        // 10. Refatore o endpoint GET /booking
        public BookingResponse GetBooking(int bookingId, string email)
        {
            var user = _context.Users.Where((u) => u.Email == email).FirstOrDefault();

            var booking = _context.Bookings.Find(bookingId);

            if (booking != null && booking.UserId == user!.UserId)
            {
                var response = from book in _context.Bookings
                    where book.BookingId == bookingId
                    select new BookingResponse
                    {
                        BookingId = book.BookingId,
                        CheckIn = book.CheckIn,
                        CheckOut = book.CheckOut,
                        GuestQuant = book.GuestQuant,
                        Room = new RoomDto
                        {
                            RoomId = book.Room!.RoomId,
                            Name = book.Room.Name,
                            Capacity = book.Room.Capacity,
                            Image = book.Room.Image,
                            Hotel = new HotelDto
                            {
                                HotelId = book.Room.Hotel!.HotelId,
                                Name = book.Room.Hotel.Name,
                                Address = book.Room.Hotel.Address,
                                CityId = book.Room.Hotel.CityId,
                                CityName = book.Room.Hotel.City!.Name,
                                State = book.Room.Hotel.City.State
                            }
                        }
                    };
                return response.First();
            }
            else
            {
                return null!;
            }
        }

        public Room GetRoomById(int RoomId)
        {
             throw new NotImplementedException();
        }

    }

}