using Entities;
using Microsoft.EntityFrameworkCore;
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

        public UsersRepository(ApplicationDbContext dbContext) {

            _dbContext = dbContext;
        }
        public Task<User?> GetUserByUsernameAsync(string username)
        {
            throw new NotImplementedException();
        }

        public Task<string?> LoginUserAsync(LoginRequest loginUser)
        {
            throw new NotImplementedException();
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

            var newUser = new User
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
