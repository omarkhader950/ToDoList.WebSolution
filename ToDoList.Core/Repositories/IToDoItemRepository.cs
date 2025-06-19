using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.DTO;
using ToDoList.Core.Entities.Entities;

namespace ToDoList.Core.Repositories
{
    public interface IToDoItemRepository : IRepository<TodoItem>
    {
        
        Task<TodoItem?> GetByIdAsync(Guid? todoItemId, Guid userId);
        Task<bool> DeleteAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin);
         Task<List<TodoItem>> GetAllDeletedItemsAsync();
        Task<TodoItem?> GetDeletedItemByIdAsync(Guid todoItemId);
        Task<bool> RestoreAsync(Guid todoItemId);
        Task<List<TodoItem>> GetPaginatedAsync(PaginationRequest request);

        Task<List<TodoItem>> GetAllByUserAsync(Guid userId);

        Task<List<TodoItem>> GetAllWithUserAsync();

        Task<List<TodoItem>> ListByIdsAsync(List<Guid> itemIds);


        Task<int> CountActiveAsync(Guid userId);

        Task AddAttachmentAsync(TodoItemAttachment attachment);




    }
}
