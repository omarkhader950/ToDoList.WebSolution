using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;


namespace ToDoList.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public AuthController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // POST: api/Auth/register
        [HttpPost("register")]
        [Authorize(Roles = "Admin")]
        public IActionResult Register([FromBody] RegisterRequest registerRequest)
        {
            try
            {
                var user = _usersService.RegisterUser(registerRequest);
                return Ok(new
                {
                    message = "User registered successfully.",
                    userId = user.Id,
                    username = user.Username
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { error = ex.Message });
            }
        }

        // POST: api/Auth/login
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest loginRequest)
        {
            var token = _usersService.LoginUser(loginRequest);

            if (token == null)
                return Unauthorized(new { error = "Invalid username or password." });

            return Ok(new { token });
        }
    }
}

//adminpasswordhash