using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.ServiceContracts;

namespace ToDoList.Infrastructure.Services
{



    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;


        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }


        public Guid? GetUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Guid.TryParse(userIdString, out var userId) ? userId : null;
        }

        public string? GetUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name;
        }

        public bool IsInRole(string role)
        {
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }

        public bool IsAdmin(string role)
        {
           
            return _httpContextAccessor.HttpContext?.User?.IsInRole(role) ?? false;
        }
    }
}
