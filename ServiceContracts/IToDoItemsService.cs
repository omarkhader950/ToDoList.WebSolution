using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;

namespace ServiceContracts
{
    public interface IToDoItemsService
    {
        /// <summary>
        /// Adds a new task to the list of TodoItems.
        /// </summary>
        /// <param name="todoItemAddRequest">Task to add</param>
        /// <returns>Returns the task details along with the newly generated ID</returns>
        ToDoItemResponse AddTodoItem(TodoItemAddRequest? todoItemAddRequest);

        /// <summary>
        /// Returns all TodoItems (admin or general case).
        /// </summary>
        /// <returns>Returns a list of TodoItemResponse objects</returns>
        List<ToDoItemResponse> GetAllTodoItems();

        /// <summary>
        /// Returns all TodoItems for a specific user.
        /// </summary>
        List<ToDoItemResponse> GetAllTodoItemsByUser(Guid userId);

        /// <summary>
        /// Returns a paginated list of TodoItems.
        /// </summary>
        List<ToDoItemResponse> GetPaginatedItems(int pageNumber, int pageSize);

        /// <summary>
        /// Returns a paginated list of TodoItems for a specific user.
        /// </summary>
        List<ToDoItemResponse> GetPaginatedItemsForUser(Guid userId, int pageNumber, int pageSize);

        /// <summary>
        /// Gets a TodoItem by ID and UserId.
        /// </summary>
        ToDoItemResponse? GetTodoItemById(Guid? todoItemId, Guid userId);

        /// <summary>
        /// Updates the specified TodoItem based on its ID.
        /// </summary>
        ToDoItemResponse UpdateTodoItem(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid userId);

        /// <summary>
        /// Deletes a TodoItem by its ID and UserId.
        /// </summary>
        bool DeleteTodoItem(Guid? todoItemId, Guid userId);

        /// <summary>
        /// Gets all deleted items (admin use).
        /// </summary>
        List<ToDoItemResponse> GetAllDeletedItems();

        /// <summary>
        /// Restores a soft-deleted TodoItem by its ID.
        /// </summary>
        bool RestoreTodoItem(Guid todoItemId);

        /// <summary>
        /// Gets a deleted TodoItem by ID.
        /// </summary>
        ToDoItemResponse? GetDeletedItemById(Guid todoItemId);



        public List<UserWithTodoItemsResponse> GetAllTodoItemsGroupedByUser();
    }
}
