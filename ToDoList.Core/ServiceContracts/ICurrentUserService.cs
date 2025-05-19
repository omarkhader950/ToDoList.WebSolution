using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.ServiceContracts
{
    public interface ICurrentUserService
    {
        Guid? GetUserId();
        string? GetUserName();

        bool IsInRole(string role);
    }
}
