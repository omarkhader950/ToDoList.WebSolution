using Entities;

using ServiceContracts;
using System;
using System.Linq;



using ServiceContracts.DTO;
using ToDoList.Infrastructure.Repositories;


namespace Services
{
    public class UsersService : IUsersService
{
    private readonly IUsersRepository _repository;
    private readonly IJwtService _jwtService;

    public UsersService(IUsersRepository repository, IJwtService jwtService)
    {
        _repository = repository;
        _jwtService = jwtService;
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
        await _dbContext.SaveChangesAsync();

        return newUser;
    }

    public async Task<string?> LoginUserAsync(LoginRequest loginUser)
    {
        var user = await _dbContext.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == loginUser.Username);

        if (user == null || !BCrypt.Net.BCrypt.Verify(loginUser.Password, user.PasswordHash))
            return null;

        return _jwtService.GenerateToken(user);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _dbContext.Users.Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Username == username);
    }
}

}
