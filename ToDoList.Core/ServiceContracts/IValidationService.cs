using Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Constants;
using ToDoList.Core.Enums;

namespace ToDoList.Core.ServiceContracts
{
    public interface IValidationService
    {
        void EnsureUserIsAuthenticated(Guid? userId);
        void ValidateUserIsAdmin(bool isAdmin);



        void ValidateUserOwnsResource<T>(T resource, Guid userId, bool isAdmin) where T : IOwnedResource;
        void ValidateUserOwnsResource<T>(T resource, Guid userId) where T : IOwnedResource;

        void ValidateTodoItemStatusForUpdate(TodoStatus? status);
        void ValidateUserIsAdminOrOwnsResource(bool isAdmin, Guid? requestedUserId, Guid? currentUserId);



         void ValidateMaxActiveItemsLimit(int currentActiveCount);
        void ValidateAllItemsAreCompleted(IEnumerable<TodoItem> items);


    }
}
