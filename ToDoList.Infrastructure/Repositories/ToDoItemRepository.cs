using Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.DTO;
using ToDoList.Core.Repositories;
using ToDoList.Infrastructure.Data;

namespace ToDoList.Infrastructure.Repositories
{
    public class ToDoItemRepository : Repository<TodoItem> ,IToDoItemRepository 
    {
        private readonly ApplicationDbContext _db;

        public ToDoItemRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

  


        

        public async Task<TodoItem?> GetTodoItemByIdAsync(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                return null;

            TodoItem? todoItem = await _db.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(temp => temp.Id == todoItemId && temp.UserId == userId);

            return todoItem == null ? null : todoItem;

                
        }




        
       

      
        public async Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid actualUserId, bool isAdmin)
        {

            if (todoItemUpdateRequest == null)
                throw new ArgumentNullException(nameof(todoItemUpdateRequest));

            // Admins can update any user's item; regular users only their own
            var matchingTodoItem = await _db.TodoItems
                .FirstOrDefaultAsync(temp =>
                    temp.Id == todoItemUpdateRequest.Id &&
                    (isAdmin || temp.UserId == actualUserId));

            if (matchingTodoItem == null)
                throw new ArgumentException("Given todo item ID doesn't exist or access denied.");

            // Update fields
            matchingTodoItem.Title = todoItemUpdateRequest.Title!;
            matchingTodoItem.Description = todoItemUpdateRequest.Description;
            matchingTodoItem.IsCompleted = todoItemUpdateRequest.IsCompleted;
            matchingTodoItem.DueDate = todoItemUpdateRequest.DueDate!.Value;

            await _db.SaveChangesAsync();

            return matchingTodoItem.Adapt<ToDoItemResponse>();
        }

      
        public async Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin)
        {
            TodoItem? todoItem;

            if (isAdmin)
            {
                // Admins can delete any item
                todoItem = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == todoItemId);
            }
            else
            {
                // Regular users can only delete their own items
                todoItem = await _db.TodoItems.FirstOrDefaultAsync(t => t.Id == todoItemId && t.UserId == tokenUserId);
            }

            if (todoItem == null)
                return false;

            todoItem.DeleteBy = tokenUserId;
            todoItem.DeleteDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

       
        public  async Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync()
        {
            return await _db.TodoItems
                 .IgnoreQueryFilters()
                 .Where(t => t.DeleteBy != null)
                 .Include(t => t.User)
                 .Select(t => t.Adapt<ToDoItemResponse>())
                 .ToListAsync();
        }

        
        public async Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId)
        {
            var item = await _db.TodoItems
               .IgnoreQueryFilters()
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Id == todoItemId);

            return item?.Adapt<ToDoItemResponse>();
        }

        
        public async Task<bool> RestoreTodoItemAsync(Guid todoItemId)
        {
            var item = await _db.TodoItems
      .IgnoreQueryFilters()
      .FirstOrDefaultAsync(t => t.Id == todoItemId && t.DeleteBy != null);

            if (item == null)
                return false;

            item.DeleteBy = null;
            item.DeleteDate = null;
            await _db.SaveChangesAsync();
            return true;
        }

        
        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request)
        {
            var query = _db.TodoItems
                .Include(t => t.User)
                .AsQueryable();

            // Apply filters on CreationDate only if values are provided
            if (request.CreatedAfter.HasValue)
            {
                query = query.Where(t => t.CreationDate >= request.CreatedAfter.Value);
            }

            if (request.CreatedBefore.HasValue)
            {
                query = query.Where(t => t.CreationDate <= request.CreatedBefore.Value);
            }

            var result = await query
                .OrderBy(t => t.Id)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(t => t.Adapt<ToDoItemResponse>())
                .ToListAsync();

            return result;
        }

      


        
        public async Task<List<TodoItem>> GetAllTodoItemsByUserAsync(Guid userId)
        {
            return await _db.TodoItems
         .Include(t => t.User)
         .Where(t => t.UserId == userId && t.DeleteBy == null).ToListAsync();
         
        }



        public async Task<List<TodoItem>> GetAllWithUserAsync()
        {
            return await _db.TodoItems.Include(t => t.User).ToListAsync();
        }


        public async Task MarkAsInProgressAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin)
        {

            var items = await _db.TodoItems
        .Where(t => itemIds.Contains(t.Id))
        .ToListAsync();

            if (!isAdmin)
            {
                // Regular users can only update their own items
                if (items.Any(t => t.UserId != currentUserId))
                    throw new UnauthorizedAccessException("You can only modify your own to-do items.");
            }

            foreach (var item in items)
            {
                item.Status = TodoStatus.InProgress;
            }

            await _db.SaveChangesAsync();
        }

        public async Task MarkAsCompletedAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin)
        {
            var items = await _db.TodoItems
        .Where(t => itemIds.Contains(t.Id))
        .ToListAsync();

            if (!isAdmin)
            {
                if (items.Any(t => t.UserId != currentUserId))
                    throw new UnauthorizedAccessException("You can only modify your own to-do items.");
            }

            foreach (var item in items)
            {
                item.Status = TodoStatus.Completed;
            }

            await _db.SaveChangesAsync();
        }

        public async Task<int> CountActiveAsync(Guid userId)
        {
            return await _db.TodoItems
            .CountAsync(t => t.UserId == userId &&
                             (t.Status == TodoStatus.New || t.Status == TodoStatus.InProgress));
        }

      
    }
}
