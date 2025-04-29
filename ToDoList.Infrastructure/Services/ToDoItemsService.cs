using Entities;
using Microsoft.EntityFrameworkCore;


using System;
using ToDoList.Infrastructure.Data;
using ToDoList.Core.DTO;
using ServiceContracts;


using Mapster;

using ServiceContracts.DTO;
using System;


namespace Services
{
    public class ToDoItemsService : IToDoItemsService
    {


        //private field
        private readonly ApplicationDbContext _db;
      //  private readonly IToDoItemsService _toDoItemsService;


        public ToDoItemsService(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
           
        }

      

       

        public async Task<List<ToDoItemResponse>> GetAllTodoItemsAsync()
        {

            return (await _db.TodoItems
                 .Include(t => t.User)
                 .ToListAsync())
             .Select(todo => todo.Adapt<ToDoItemResponse>())
             .ToList();

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

        public async Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid userId)
        {
            if (todoItemUpdateRequest == null)
                throw new ArgumentNullException(nameof(todoItemUpdateRequest));

            var matchingTodoItem = await _db.TodoItems
                .FirstOrDefaultAsync(temp => temp.Id == todoItemUpdateRequest.Id && temp.UserId == userId);

            if (matchingTodoItem == null)
                throw new ArgumentException("Given todo item ID doesn't exist.");

            matchingTodoItem.Title = todoItemUpdateRequest.Title;
            matchingTodoItem.Description = todoItemUpdateRequest.Description;
            matchingTodoItem.IsCompleted = todoItemUpdateRequest.IsCompleted;

            await _db.SaveChangesAsync();

            return matchingTodoItem.Adapt<ToDoItemResponse>(); 
        }

        public async Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                throw new ArgumentNullException(nameof(todoItemId));

            var todoItem = await _db.TodoItems
                .FirstOrDefaultAsync(temp => temp.Id == todoItemId && temp.UserId == userId);

            if (todoItem == null)
                return false;

            todoItem.DeleteBy = todoItem.UserId;
            todoItem.DeleteDate = DateTime.UtcNow;

            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<ToDoItemResponse> AddTodoItemAsync(TodoItemAddRequest? todoItemAddRequest, Guid userId)
        {

            
            var todoItem = todoItemAddRequest.Adapt<TodoItem>();
            todoItem.UserId = userId;

            await _db.TodoItems.AddAsync(todoItem);
            await _db.SaveChangesAsync();

            return todoItem.Adapt<ToDoItemResponse>(); 
        }


        /// <summary>
        /// Retrieves all soft-deleted todo items by ignoring global query filters.
        /// </summary>
        /// <returns>List of deleted todo items mapped to response models.</returns>
        public async Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync()
        {
            return await _db.TodoItems
                 .IgnoreQueryFilters()
                 .Where(t => t.DeleteBy != null)
                 .Include(t => t.User)
                 .Select(t => t.Adapt<ToDoItemResponse>())
                 .ToListAsync();


        }

        /// <summary>
        /// Restores a soft-deleted todo item by its ID.
        /// Ignores global query filters to access the deleted item.
        /// </summary>
        /// <param name="todoItemId">The ID of the todo item to restore.</param>
        /// <returns>True if the item was found and restored; otherwise, false.</returns>
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


        /// <summary>
        /// Retrieves a single soft-deleted todo item by its ID by ignoring global query filters.
        /// </summary>
        /// <param name="todoItemId">The ID of the soft-deleted todo item to retrieve.</param>
        /// <returns>The soft-deleted todo item mapped to a response model, or null if not found.</returns>
        public async Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId)
        {
            var item = await _db.TodoItems
               .IgnoreQueryFilters()
               .Include(t => t.User)
               .FirstOrDefaultAsync(t => t.Id == todoItemId);

            return item?.Adapt<ToDoItemResponse>();
        }



       
        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(int pageNumber, int pageSize)
        {
            return await _db.TodoItems
               .Include(t => t.User)
               .OrderBy(t => t.Id)
               .Skip((pageNumber - 1) * pageSize)
               .Take(pageSize)
               .Select(t => t.Adapt<ToDoItemResponse>())
               .ToListAsync();

        }

        public async Task<List<ToDoItemResponse>> GetPaginatedItemsForUserAsync(Guid userId, int pageNumber, int pageSize)
        {
            return await _db.TodoItems
              .Include(t => t.User)
              .Where(t => t.UserId == userId && t.DeleteBy == null) // Active items only
              .OrderBy(t => t.Id)
              .Skip((pageNumber - 1) * pageSize)
              .Take(pageSize)
              .Select(t => t.Adapt<ToDoItemResponse>())
              .ToListAsync();
        }



        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId)
        {
            return await _db.TodoItems
            .Include(t => t.User)
            .Where(t => t.UserId == userId && t.DeleteBy == null)
            .Select(t => t.Adapt<ToDoItemResponse>())
            .ToListAsync();
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
                        IsCompleted = t.IsCompleted,
                        DueDate = t.DueDate
                    }).ToList()
                })
                .ToList();
        }




    }
}
