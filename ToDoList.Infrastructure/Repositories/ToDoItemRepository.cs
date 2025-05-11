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
    public class ToDoItemRepository : IToDoItemRepository
    {
        private readonly ApplicationDbContext _db;

        public ToDoItemRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<TodoItem>> GetAllAsync()
        {
            return await _db.TodoItems.Include(t => t.User).ToListAsync();
        }


        

        public async Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                return null;

            var todoItem = await _db.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(temp => temp.Id == todoItemId && temp.UserId == userId);

            return todoItem == null ? null : todoItem.Adapt<ToDoItemResponse>();
        }




        
        public async Task<ToDoItemResponse> AddTodoItemAsync(TodoItemAddRequest? todoItemAddRequest, Guid userId)
        {


            if (todoItemAddRequest == null)
                throw new ArgumentNullException(nameof(todoItemAddRequest));

            // Count active items (Pending or InProgress)
            var activeItemCount = await _db.TodoItems
                .CountAsync(t => t.UserId == userId &&
                                 (t.Status == TodoStatus.Pending || t.Status == TodoStatus.InProgress));

            if (activeItemCount >= 10)
                throw new InvalidOperationException("User already has 10 active ToDo items (Pending or InProgress).");

            // Map and prepare item
            var todoItem = todoItemAddRequest.Adapt<TodoItem>();
            todoItem.UserId = userId;
            todoItem.CreationDate = DateTime.UtcNow;

            await _db.TodoItems.AddAsync(todoItem);
            await _db.SaveChangesAsync();

            return todoItem.Adapt<ToDoItemResponse>();

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

      


        
        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId)
        {
            return await _db.TodoItems
         .Include(t => t.User)
         .Where(t => t.UserId == userId && t.DeleteBy == null)
         .Select(t => t.Adapt<ToDoItemResponse>())
         .ToListAsync();
        }

        public async Task<List<TodoItem>> GetAllWithUserAsync()
        {
            return await _db.TodoItems.Include(t => t.User).ToListAsync();
        }



        public async Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync()
        {
            var items = await _db.TodoItems
              .Include(t => t.User)
              .ToListAsync();

            return items
                .GroupBy(t => new { t.User.Id, t.User.Username })
                .Select(g => new UserWithTodoItemsResponse
                {
                    UserId = g.Key.Id,
                    UserName = g.Key.Username,
                    TodoItems = g.Select(t => new ToDoItemResponse
                    {
                        Id = t.Id,
                        Title = t.Title,
                        Description = t.Description,
                        Status = t.Status
                        ,


                        DueDate = t.DueDate
                    }).ToList()
                })
                .ToList();

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
    }
}
