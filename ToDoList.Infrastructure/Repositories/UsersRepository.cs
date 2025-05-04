using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {

        private readonly ApplicationDbContext _dbContext;
        private readonly IJwtService _jwt;

        public UsersRepository(ApplicationDbContext dbContext, IJwtService jwt) {

            _dbContext = dbContext;
            _jwt = jwt;
        }
        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _dbContext.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
        }

        public async Task<string?> LoginUserAsync(LoginRequest loginUser)
        {
            var user = await _dbContext.Users.Include(u => u.Role)
          .FirstOrDefaultAsync(u => u.Username == loginUser.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
                return null;

            return _jwt.GenerateToken(user);
        }

        public async Task<User> RegisterUserAsync(RegisterRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Username))
                throw new ArgumentException("Username is required.");

            if (string.IsNullOrWhiteSpace(user.Password))
                throw new ArgumentException("Password is required.");

            if (await _dbContext.Users.AnyAsync(u => u.Username == user.Username))
                throw new InvalidOperationException("Username is already taken.");

            var defaultRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            if (defaultRole == null)
                throw new Exception("Default role 'User' not found. Please seed the roles properly.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);

            User newUser = new User
            {
                Id = Guid.NewGuid(),
                Username = user.Username,
                PasswordHash = hashedPassword,
                RoleId = defaultRole.Id
            };

            _dbContext.Users.Add(newUser);
            _dbContext.SaveChangesAsync();

            return newUser;
        }
    }
}
