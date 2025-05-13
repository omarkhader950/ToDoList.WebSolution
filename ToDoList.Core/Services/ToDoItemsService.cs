using ToDoList.Core.DTO;
using ServiceContracts;
using ServiceContracts.DTO;
using ToDoList.Core.Repositories;
using Entities;
using ToDoList.Infrastructure.Mapping;
using ToDoList.Core.Enums;




namespace Services
{
    public class ToDoItemsService : IToDoItemsService 
    {


        private readonly IToDoItemRepository _repository;
        private readonly IMappingService _mapper;


        public ToDoItemsService(IToDoItemRepository repository, IMappingService mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }


        public async Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId, Guid userId)
        {
            if (todoItemId == null)
                return null;



            var result = await _repository.GetTodoItemByIdAsync(todoItemId,userId);

          return   _mapper.Map<TodoItem, ToDoItemResponse>(result);

            
        }

        public async Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? request, Guid actualUserId, bool isAdmin)
        {

            if (request == null) throw new ArgumentNullException(nameof(request));

            var item = await _repository.GetByIdAsync(request.Id);

            if (item == null || (!isAdmin && item.UserId != actualUserId))
                throw new ArgumentException("Given todo item ID doesn't exist or access denied.");

            item.Title = request.Title!;
            item.Description = request.Description;
            item.IsCompleted = request.IsCompleted;
            item.DueDate = request.DueDate!.Value;

            await _repository.SaveChangesAsync();
            return _mapper.Map<TodoItem, ToDoItemResponse>(item);
             
        }


  
        public async Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin)
        {
            if (todoItemId == Guid.Empty)
                throw new ArgumentException("Todo item ID is required.", nameof(todoItemId));


            return await _repository.DeleteTodoItemAsync(todoItemId, tokenUserId, isAdmin);
        }



        public async Task<List<ToDoItemResponse>> AddTodoItemsAsync(
     List<TodoItemAddRequest> requestList,
     Guid tokenUserId,
     bool isAdmin)
        {
            var entities = requestList.Select(request =>
            {
                var entity = _mapper.Map<TodoItemAddRequest, TodoItem>(request);
                entity.UserId = isAdmin && request.UserId.HasValue
                    ? request.UserId.Value
                    : tokenUserId;
                entity.CreationDate = DateTime.UtcNow;
                return entity;
            }).ToList();

            var result = new List<TodoItem>();

            foreach (var item in entities)
            {
                int activeCount = await _repository.CountActiveAsync(item.UserId);
                if (activeCount >= 10)
                    throw new InvalidOperationException("User already has 10 active ToDo items.");

                await _repository.AddAsync(item);
                result.Add(item);
            }

            await _repository.SaveChangesAsync();

            return _mapper.MapList<TodoItem, ToDoItemResponse>(result);
        }



        /// <summary>
        /// Retrieves all soft-deleted todo items by ignoring global query filters.
        /// </summary>
        /// <returns>List of deleted todo items mapped to response models.</returns>
        public async Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync()
        {


            List<TodoItem> todoItems =  await _repository.GetAllDeletedItemsAsync();

            return _mapper.MapList<TodoItem, ToDoItemResponse>(todoItems);


        }

        /// <summary>
        /// Restores a soft-deleted todo item by its ID.
        /// Ignores global query filters to access the deleted item.
        /// </summary>
        /// <param name="todoItemId">The ID of the todo item to restore.</param>
        /// <returns>True if the item was found and restored; otherwise, false.</returns>
        public async Task<bool> RestoreTodoItemAsync(Guid todoItemId)
        {
        
            return await _repository.RestoreTodoItemAsync(todoItemId);
        }


        /// <summary>
        /// Retrieves a single soft-deleted todo item by its ID by ignoring global query filters.
        /// </summary>
        /// <param name="todoItemId">The ID of the soft-deleted todo item to retrieve.</param>
        /// <returns>The soft-deleted todo item mapped to a response model, or null if not found.</returns>
        public async Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId)
        {
           



            TodoItem? todoItem =  await _repository.GetDeletedItemByIdAsync(todoItemId);

            if (todoItem == null) return null;


            return _mapper.Map<TodoItem, ToDoItemResponse>(todoItem);

        }



       
        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request)
        {
 
            
            List<TodoItem> todoItems =  await _repository.GetPaginatedItemsAsync(request);
            return _mapper.MapList<TodoItem, ToDoItemResponse>(todoItems);

        }


        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId)
        {

            List<TodoItem> result = await _repository.GetAllTodoItemsByUserAsync(userId);
            return  _mapper.MapList<TodoItem, ToDoItemResponse>(result);

        }


     
        public async Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync()
        {

            var items = await _repository.GetAllWithUserAsync();

            return items
                .GroupBy(t => new { t.User.Id, t.User.Username })
                .Select(g => new UserWithTodoItemsResponse
                {
                    UserId = g.Key.Id,
                    UserName = g.Key.Username,
                    TodoItems = g.Select(t => _mapper.Map<TodoItem, ToDoItemResponse>(t)).ToList()
                })
                .ToList();



           
        }

        public async Task MarkAsInProgressAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin)
        {
            var items = await _repository.GetItemsByIdsAsync(itemIds);

            if (!isAdmin)
            {
                if (items.Any(t => t.UserId != currentUserId))
                    throw new UnauthorizedAccessException("You can only modify your own to-do items.");
            }

            foreach (var item in items)
            {
                item.Status = TodoStatus.InProgress;
            }

            await _repository.SaveChangesAsync(); 
        }


        public async Task MarkAsCompletedAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin)
        {
            var items = await _repository.GetItemsByIdsAsync(itemIds);

            if (!isAdmin && items.Any(t => t.UserId != currentUserId))
                throw new UnauthorizedAccessException("You can only modify your own to-do items.");

            foreach (var item in items)
            {
                item.Status = TodoStatus.Completed;
            }

            await _repository.SaveChangesAsync();
        }




    }
}
