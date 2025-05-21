using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Constants;
using ToDoList.Core.ServiceContracts;

namespace ToDoList.Infrastructure.Services
{
    public class UserValidator : IUserValidator
    {

        private readonly ICurrentUserService _currentUser;


        public UserValidator( ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }

    

        public void ValidateAdminAccess(bool? isAdmin)
        {
            if (isAdmin == null || isAdmin == false)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);

        }

        public void ValidateUserAccess(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);
        }
    }
}
