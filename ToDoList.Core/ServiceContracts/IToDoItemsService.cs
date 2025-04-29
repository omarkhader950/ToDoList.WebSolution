using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using ToDoList.Core.DTO;

namespace ServiceContracts
{
    public interface IToDoItemsService
    {
        /// <summary>
        /// Adds a new task to the list of TodoItems.
        /// </summary>
        /// <param name="todoItemAddRequest">Task to add</param>
        /// <returns>Returns the task details along with the newly generated ID</returns>
        Task<ToDoItemResponse> AddTodoItemAsync(TodoItemAddRequest? todoItemAddRequest);


        /// <summary>
        /// Returns all TodoItems (admin or general case).
        /// </summary>
        /// <returns>Returns a list of TodoItemResponse objects</returns>
        Task<List<ToDoItemResponse>> GetAllTodoItemsAsync();


        /// <summary>
        /// Returns all TodoItems for a specific user.
        /// </summary>
        Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId);

        /// <summary>
        /// Returns a paginated list of TodoItems.
        /// </summary>
        Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(int pageNumber, int pageSize);


        /// <summary>
        /// Returns a paginated list of TodoItems for a specific user.
        /// </summary>
        Task<List<ToDoItemResponse>> GetPaginatedItemsForUserAsync(Guid userId, int pageNumber, int pageSize);

        /// <summary>
        /// Gets a TodoItem by ID and UserId.
        /// </summary>
        Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId, Guid userId);

        /// <summary>
        /// Updates the specified TodoItem based on its ID.
        /// </summary>
        Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid userId);

        /// <summary>
        /// Deletes a TodoItem by its ID and UserId.
        /// </summary>
        Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid userId);

        /// <summary>
        /// Gets all deleted items (admin use).
        /// </summary>
        Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync();

        /// <summary>
        /// Restores a soft-deleted TodoItem by its ID.
        /// </summary>
        Task<bool> RestoreTodoItemAsync(Guid todoItemId);

        /// <summary>
        /// Gets a deleted TodoItem by ID.
        /// </summary>
        Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId);



        Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync();
    }
}
