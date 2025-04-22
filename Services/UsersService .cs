using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using System;
using System.Linq;
using BCrypt.Net;
using ServiceContracts.DTO;

namespace Services
{
    public class UsersService : IUsersService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtService _jwtService;

        public UsersService(ApplicationDbContext dbContext, IJwtService jwtService)
        {
            _dbContext = dbContext;
            _jwtService = jwtService;
        }
        // regester requerst DTO 
        public User RegisterUser(RegisterRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required.");

            if (_dbContext.Users.Any(u => u.Username == user.Username))
                throw new InvalidOperationException("Username is already taken.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            var newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                PasswordHash = hashedPassword,
                Role = "User" // default role
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChanges();

            return newUser;
        }



        //LoginRequest DTO
        public string? LoginUser(LoginRequest loginUser)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Username == loginUser.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
                return null; // invalid credentials

            return _jwtService.GenerateToken(user);
        }

        public User? GetUserByUsername(string username)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Username == username);
        }
    }
}
