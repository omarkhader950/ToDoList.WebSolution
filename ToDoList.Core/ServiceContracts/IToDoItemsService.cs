using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using ToDoList.Core.DTO;

namespace ServiceContracts
{
    public interface IToDoItemsService
    {
        

        Task<List<ToDoItemResponse>> AddTodoItemsAsync(
     List<TodoItemAddRequest> requestList,
     Guid tokenUserId,
     bool isAdmin);





        /// <summary>
        /// Returns all TodoItems for a specific user.
        /// </summary>
        Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId);

        /// <summary>
        /// Returns a paginated list of TodoItems.
        /// </summary>
        Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request);


        /// <summary>
        /// Gets a TodoItem by ID and UserId.
        /// </summary>
        Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId, Guid userId);

       


        Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid actualUserId, bool isAdmin);

        /// <summary>
        /// Deletes a TodoItem by its ID and UserId.
        /// </summary>
        Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin);

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



         Task MarkAsInProgressAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin);

        Task MarkAsCompletedAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin);

        Task MarkAsNewAsync(List<Guid> itemIds, Guid currentUserId);

        Task ResetCompletedToInProgressAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin);
    }

}
