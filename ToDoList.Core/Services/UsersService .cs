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

    public User RegisterUserAsync(RegisterRequest user)
    {
        

        return  _repository.RegisterUserAsync(user);
    }

    public async Task<string?> LoginUserAsync(LoginRequest loginUser)
    {
        return await _repository.LoginUserAsync(loginUser);
    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _repository.GetUserByUsernameAsync(username);
    }
}

}
