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
        Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin);
         Task<List<TodoItem>> GetAllDeletedItemsAsync();
        Task<TodoItem?> GetDeletedItemByIdAsync(Guid todoItemId);
        Task<bool> RestoreTodoItemAsync(Guid todoItemId);
        Task<List<TodoItem>> GetPaginatedItemsAsync(PaginationRequest request);

        Task<List<TodoItem>> GetAllTodoItemsByUserAsync(Guid userId);

        Task<List<TodoItem>> GetAllWithUserAsync();

        Task<List<TodoItem>> GetItemsByIdsAsync(List<Guid> itemIds);


        Task<int> CountActiveAsync(Guid userId);
        


    }
}
