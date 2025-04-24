using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using ServiceContracts;
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

        private ToDoItemResponse ConvertTodoItemToDoItemResponse(TodoItem todoItem)
        {
            ToDoItemResponse toDoItemResponse = todoItem.ToTodoItemResponse();
            return toDoItemResponse;
        }

       

        public List<ToDoItemResponse> GetAllTodoItems()
        {

            return _db.TodoItems
          .Include(t => t.User) 
          .ToList()
          .Select(temp => ConvertTodoItemToDoItemResponse(temp))
          .ToList();

        }



        public ToDoItemResponse? GetTodoItemById(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                return null;

            TodoItem? todoItem = _db.TodoItems.Include(t => t.User)
              .FirstOrDefault(temp => temp.Id == todoItemId && temp.UserId == userId);
            if (todoItem == null)
                return null;

            return ConvertTodoItemToDoItemResponse(todoItem);
        }

        public ToDoItemResponse UpdateTodoItem(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid userId)
        {
            if (todoItemUpdateRequest == null)
                throw new ArgumentNullException(nameof(TodoItem));

          

            //get matching  object to update
            TodoItem? matchingTodoItem = _db.TodoItems
              .FirstOrDefault(temp => temp.Id == todoItemUpdateRequest.Id && temp.UserId == userId);
            if (matchingTodoItem == null)
            {
                throw new ArgumentException("Given person id doesn't exist");
            }

            //update all details
            matchingTodoItem.Title = todoItemUpdateRequest.Title;
            matchingTodoItem.Description = todoItemUpdateRequest.Description;
            matchingTodoItem.IsCompleted = todoItemUpdateRequest.IsCompleted;
          //  matchingTodoItem.DueDate = todoItemUpdateRequest.DueDate; 


            _db.SaveChanges(); //UPDATE

            return ConvertTodoItemToDoItemResponse(matchingTodoItem);
        }

        public bool DeleteTodoItem(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                throw new ArgumentNullException(nameof(todoItemId));

            // Because of the global filter, this will auto-exclude deleted items
            var todoItem = _db.TodoItems.FirstOrDefault(temp => temp.Id == todoItemId && temp.UserId == userId);
            if (todoItem == null)
                return false;

            // Instead of removing it, just mark as deleted
            todoItem.IsDeleted = true;
            _db.SaveChanges();

            return true;
        }

        public ToDoItemResponse AddTodoItem(TodoItemAddRequest? todoItemAddRequest)
        {
            //convert personAddRequest into Person type
            TodoItem todoItem = todoItemAddRequest.ConvertToTodoItem();

            //generate 
           // todoItem.Id = Guid.NewGuid();
            todoItem.UserId = todoItemAddRequest.UserId; //  Set user ownership


            //add person object to persons list
            _db.TodoItems.Add(todoItem);
            _db.SaveChanges();
            //_db.sp_InsertPerson(person);

            //convert the Person object into PersonResponse type
            return ConvertTodoItemToDoItemResponse(todoItem);
        }


        /// <summary>
        /// Retrieves all soft-deleted todo items by ignoring global query filters.
        /// </summary>
        /// <returns>List of deleted todo items mapped to response models.</returns>
        public List<ToDoItemResponse> GetAllDeletedItems()
        {
            return _db.TodoItems
                .Include(t => t.User)
                .IgnoreQueryFilters()
                .Where(t => t.IsDeleted)
                .Select(t => t.ToTodoItemResponse())
                .ToList();


        }

        /// <summary>
        /// Restores a soft-deleted todo item by its ID.
        /// Ignores global query filters to access the deleted item.
        /// </summary>
        /// <param name="todoItemId">The ID of the todo item to restore.</param>
        /// <returns>True if the item was found and restored; otherwise, false.</returns>
        public bool RestoreTodoItem(Guid todoItemId)
        {
            var item = _db.TodoItems
                .IgnoreQueryFilters()
                .FirstOrDefault(t => t.Id == todoItemId && t.IsDeleted);

            if (item == null) return false;

            item.IsDeleted = false;
            _db.SaveChanges();
            return true;
        }


        /// <summary>
        /// Retrieves a single soft-deleted todo item by its ID by ignoring global query filters.
        /// </summary>
        /// <param name="todoItemId">The ID of the soft-deleted todo item to retrieve.</param>
        /// <returns>The soft-deleted todo item mapped to a response model, or null if not found.</returns>
        public ToDoItemResponse? GetDeletedItemById(Guid todoItemId)
        {
            var item = _db.TodoItems
                .Include(t => t.User)
                .IgnoreQueryFilters()
                .FirstOrDefault(t => t.Id == todoItemId && t.IsDeleted);

            return item?.ToTodoItemResponse();
        }



       
        public List<ToDoItemResponse> GetPaginatedItems(int pageNumber, int pageSize)
        {
            return _db.TodoItems
                .Include(t => t.User)
                .OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize).Select(t => t.ToTodoItemResponse()).ToList();
     
        }

        public List<ToDoItemResponse> GetPaginatedItemsForUser(Guid userId, int pageNumber, int pageSize)
        {
            return _db.TodoItems
                .Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .OrderBy(t => t.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(t => ConvertTodoItemToDoItemResponse(t))
                .ToList();
        }



        public List<ToDoItemResponse> GetAllTodoItemsByUser(Guid userId)
        {
            return _db.TodoItems.Include(t => t.User)
                .Where(t => t.UserId == userId && !t.IsDeleted)
                .Select(t => t.ToTodoItemResponse())
                .ToList();
        }



public List<UserWithTodoItemsResponse> GetAllTodoItemsGroupedByUser()
{
    return _db.TodoItems
        .Include(t => t.User)
        
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
