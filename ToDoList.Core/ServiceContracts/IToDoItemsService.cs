using Entities;
using Microsoft.AspNetCore.Http;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using ToDoList.Core.DTO;
using ToDoList.Core.DTO.AttachmentDto;

namespace ServiceContracts
{
    public interface IToDoItemsService
    {
        

        Task<List<ToDoItemResponse>> AddTodoItemsAsync(
     List<TodoItemAddRequest> requestList);





        /// <summary>
        /// Returns all TodoItems for a specific user.
        /// </summary>
        Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync();

        /// <summary>
        /// Returns a paginated list of TodoItems.
        /// </summary>
        Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request);


        /// <summary>
        /// Gets a TodoItem by ID and UserId.
        /// </summary>
        Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId);

       


        Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest);

        /// <summary>
        /// Deletes a TodoItem by its ID and UserId.
        /// </summary>
        Task<bool> DeleteTodoItemAsync(Guid? todoItemId);

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



         Task MarkAsInProgressAsync(List<Guid> itemIds);

        Task MarkAsCompletedAsync(List<Guid> itemIds);

        Task MarkAsNewAsync(List<Guid> itemIds);

        Task ResetCompletedToInProgressAsync(List<Guid> itemIds);

        Task<TodoItemAttachmentResponse> UploadAttachmentAsync(Guid todoItemId, IFormFile file);



    }

}
