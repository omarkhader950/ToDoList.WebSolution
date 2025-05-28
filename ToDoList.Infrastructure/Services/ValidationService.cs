using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Constants;
using ToDoList.Core.Enums;
using ToDoList.Core.ServiceContracts;

namespace ToDoList.Infrastructure.Services
{
    public class ValidationService : IValidationService
    {

        private readonly ICurrentUserService _currentUser;


        public ValidationService(ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
        }



        public void EnsureUserIsAuthenticated(Guid? userId)
        {
            if (userId == null || userId == Guid.Empty)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);
        }

        public void ValidateMaxActiveItemsLimit(int currentActiveCount)
        {

            if (currentActiveCount >= ValidationConstants.MaxActiveItemsLimit)
                throw new InvalidOperationException(ErrorMessages.UserHasMaxActiveItems);
        }

        public void ValidateTodoItemStatusForUpdate(TodoStatus? status)
        {
            if (status == TodoStatus.Completed || status == null)
                throw new InvalidOperationException(ErrorMessages.CannotUpdateCompletedTask);
        }



        public void ValidateUserIsAdmin(bool isAdmin)
        {
            if (!isAdmin)
                throw new UnauthorizedAccessException(ErrorMessages.AdminsOnly);
        }

        public void ValidateUserIsAdminOrOwnsResource(bool isAdmin, Guid? requestedUserId, Guid? currentUserId)
        {
            if (!isAdmin && requestedUserId.HasValue && requestedUserId != currentUserId)
            {
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);
            }
        }

        public void ValidateUserOwnsResource<T>(T resource, Guid userId, bool isAdmin) where T : IOwnedResource
        {
            if (resource == null || (!isAdmin && resource.OwnerId != userId))
                throw new ArgumentException(ErrorMessages.TodoItemNotFoundOrAccessDenied);
        }

        public void ValidateUserOwnsResource<T>(T resource, Guid userId) where T : IOwnedResource
        {
            if (resource == null || (resource.OwnerId != userId))
                throw new ArgumentException(ErrorMessages.TodoItemNotFoundOrAccessDenied);
        }

        public void ValidateAllItemsAreCompleted(IEnumerable<TodoItem> items)
        {
            if (items.Any(item => item.Status != TodoStatus.Completed))
                throw new InvalidOperationException(ErrorMessages.CannotResetUnlessCompleted);
        }

        public void EnsureNotNull(object? obj, string parameterName)
        {
            if (obj == null)
                throw new ArgumentNullException(parameterName);
        }


    }
}
