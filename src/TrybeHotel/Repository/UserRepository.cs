using TrybeHotel.Models;
using TrybeHotel.Dto;

namespace TrybeHotel.Repository
{
    public class UserRepository : IUserRepository
    {
        protected readonly ITrybeHotelContext _context;
        public UserRepository(ITrybeHotelContext context)
        {
            _context = context;
        }
        public UserDto GetUserById(int userId)
        {
            throw new NotImplementedException();
        }

        public UserDto Login(LoginDto login)
        {
            var userLogin = _context.Users.FirstOrDefault(u => u.Email == login.Email && u.Password == login.Password);
        
            if (userLogin == null)
            {
                return null!;
            }

            return new UserDto
            {
                UserId = userLogin.UserId,
                Name = userLogin.Name,
                Email = userLogin.Email,
                UserType = userLogin.UserType
            };
        }
        public UserDto Add(UserDtoInsert user)
        {
            if (_context.Users.Any(u => u.Email == user.Email))
            {
                throw new ApplicationException("User email already exists");
            }

            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
                Password = user.Password,
                UserType = "client"
            };

            _context.Users.Add(newUser);
            _context.SaveChanges();

            var userDto = new UserDto
            {
                UserId = newUser.UserId,
                Name = newUser.Name,
                Email = newUser.Email,
                UserType = newUser.UserType
            };

            return userDto;
        }

        public UserDto GetUserByEmail(string userEmail)
        {
             throw new NotImplementedException();
        }

        public IEnumerable<UserDto> GetUsers()
        {
            var users = _context.Users.Select(u => new UserDto
            {
                UserId = u.UserId,
                Name = u.Name,
                Email = u.Email,
                UserType = u.UserType
            });

            return users;
        }

    }
}