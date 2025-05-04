using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Infrastructure.Repositories
{
    public interface IUsersRepository
    {

        User RegisterUserAsync(RegisterRequest user);
        Task<string> LoginUserAsync(LoginRequest loginUser);
        Task<User> GetUserByUsernameAsync(string username);
    }
}
