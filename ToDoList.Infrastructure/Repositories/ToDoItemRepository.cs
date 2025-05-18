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
using ToDoList.Core.Enums;
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

  


        

        public async Task<TodoItem?> GetByIdAsync(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                return null;

            TodoItem? todoItem = await _db.TodoItems
                .Include(t => t.User)
                .FirstOrDefaultAsync(temp => temp.Id == todoItemId && temp.UserId == userId);

            return todoItem == null ? null : todoItem;

                
        }


        public async Task<bool> DeleteAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin)
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

       
        public  async Task<List<TodoItem>> GetAllDeletedItemsAsync()
        {
            return await _db.TodoItems
                 .IgnoreQueryFilters()
                 .Where(t => t.DeleteBy != null)
                 .Include(t => t.User)
                 .ToListAsync();

        }

        
        public async Task<TodoItem?> GetDeletedItemByIdAsync(Guid todoItemId)
        {
            var item = await _db.TodoItems
               .IgnoreQueryFilters()
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Id == todoItemId);

            return item;
        }

        
        public async Task<bool> RestoreAsync(Guid todoItemId)
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


        public async Task<List<TodoItem>> GetPaginatedAsync(PaginationRequest request)
        {
            var query = _db.TodoItems
                .Include(t => t.User)
                .AsQueryable();

            // Filter by creation date range
            if (request.CreatedDateRange?.From != null)
                query = query.Where(t => t.CreationDate >= request.CreatedDateRange.From.Value);

            if (request.CreatedDateRange?.To != null)
                query = query.Where(t => t.CreationDate <= request.CreatedDateRange.To.Value);

            // Filter by due date range
            if (request.DueDateRange?.From != null)
                query = query.Where(t => t.DueDate >= request.DueDateRange.From.Value);

            if (request.DueDateRange?.To != null)
                query = query.Where(t => t.DueDate <= request.DueDateRange.To.Value);

            // Filter by statuses
            if (request.Statuses != null && request.Statuses.Any())
                query = query.Where(t => request.Statuses.Contains(t.Status));

            // Filter by title (case-insensitive contains)
            if (!string.IsNullOrWhiteSpace(request.Title))
            {
                var keyword = request.Title.Trim().ToLower();
                query = query.Where(t => t.Title.ToLower().Contains(keyword));
            }

            // Apply dynamic sorting
            query = ApplySorting(query, request.SortBy, request.SortAscending);

            // Apply pagination
            var result = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            return result;
        }

        // 1- convert swich case to dectionary
        // 2- orderby dynamic
        private IQueryable<TodoItem> ApplySorting(IQueryable<TodoItem> query, string? sortBy, bool sortAscending)
        {
            sortBy = string.IsNullOrWhiteSpace(sortBy) ? "Id" : sortBy.ToLower();

            return sortBy switch
            {
                "title" => sortAscending ? query.OrderBy(t => t.Title) : query.OrderByDescending(t => t.Title),
                "status" => sortAscending ? query.OrderBy(t => t.Status) : query.OrderByDescending(t => t.Status),
                "duedate" => sortAscending ? query.OrderBy(t => t.DueDate) : query.OrderByDescending(t => t.DueDate),
                "creationdate" => sortAscending ? query.OrderBy(t => t.CreationDate) : query.OrderByDescending(t => t.CreationDate),
                "id" => sortAscending ? query.OrderBy(t => t.Id) : query.OrderByDescending(t => t.Id),
                _ => sortAscending ? query.OrderBy(t => t.Id) : query.OrderByDescending(t => t.Id)
            };
        }








        public async Task<List<TodoItem>> GetAllByUserAsync(Guid userId)
        {
            return await _db.TodoItems
         .Include(t => t.User)
         .Where(t => t.UserId == userId && t.DeleteBy == null).ToListAsync();
         
        }



        public async Task<List<TodoItem>> GetAllWithUserAsync()
        {
            return await _db.TodoItems.Include(t => t.User).ToListAsync();
        }

        public async Task<List<TodoItem>> ListByIdsAsync(List<Guid> itemIds)
        {
            return await _db.TodoItems
                .Where(t => itemIds.Contains(t.Id))
                .ToListAsync();
        }

        public async Task<int> CountActiveAsync(Guid userId)
        {
            return await _db.TodoItems
            .CountAsync(t => t.UserId == userId &&
                             (t.Status == TodoStatus.New || t.Status == TodoStatus.InProgress));
        }

      
    }
}
