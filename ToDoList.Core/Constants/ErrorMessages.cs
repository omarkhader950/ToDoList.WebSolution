using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Core.Constants
{
    public static class ErrorMessages
    {
        public const string AdminOnlyAction = "Only admins can change completed items.";
        public const string UserNotAuthenticated = "User is not authenticated.";
        public const string TodoItemNotFoundOrAccessDenied = "Given todo item ID doesn't exist or access denied.";
        public const string CannotUpdateCompletedTask = "Cannot update a task that is already marked as Completed.";
        public const string TodoItemIdIsRequired = "Todo item ID is required.";
        public const string UserHasMaxActiveItems = "User already has 10 active ToDo items.";
        public const string AdminsOnly = "Admins only";
        public const string ModifyOwnItemsOnly = "You can only modify your own to-do items.";

        public const string UnauthorizedUser = "Invalid or missing user ID.";
        public const string MaxTodoItemsReached = "User already has 10 active ToDo items.";
        public const string InvalidItemStatus = "Cannot reset item because its status is not 'Completed'.";

        public const string CannotResetUnlessCompleted = "Cannot reset item because its status is not 'Completed'.";
        public const string InvalidPagination = "Page number and page size must be greater than 0.";
        public const string EmptyTodoItemList = "At least one ToDo item must be provided.";
    }
}
