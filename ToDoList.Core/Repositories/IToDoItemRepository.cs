using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.DTO;

namespace ToDoList.Core.Repositories
{
    public interface IToDoItemRepository : IRepository<TodoItem>
    {
        
        Task<TodoItem?> GetTodoItemByIdAsync(Guid? todoItemId, Guid userId);
        Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid actualUserId, bool isAdmin);
        Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin);
         Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync();
        Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId);
        Task<bool> RestoreTodoItemAsync(Guid todoItemId);
        Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request);

        Task<List<TodoItem>> GetAllTodoItemsByUserAsync(Guid userId);

        Task<List<TodoItem>> GetAllWithUserAsync();

        


        Task MarkAsInProgressAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin);

        Task MarkAsCompletedAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin);



        Task<int> CountActiveAsync(Guid userId);
        


    }
}
