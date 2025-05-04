using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IUsersService
    {
        /// <summary>
        /// Registers a new user
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="password">Raw password of the user</param>
        /// <returns>Returns the registered user entity</returns>
        Task<User> RegisterUserAsync(RegisterRequest user);


        /// <summary>
        /// Validates the user login and returns a JWT if valid
        /// </summary>
        /// <param name="username">Username to validate</param>
        /// <param name="password">Password to validate</param>
        /// <returns>Returns JWT token if valid, otherwise null</returns>
        Task<string?> LoginUserAsync(LoginRequest loginUser);

        /// <summary>
        /// Returns the user object by username
        /// </summary>
        /// <param name="username">Username to look up</param>
        /// <returns>Returns the user object or null</returns>
        Task<User?> GetUserByUsernameAsync(string username);
    }
}

