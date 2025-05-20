using ToDoList.Core.DTO;
using ServiceContracts;
using ServiceContracts.DTO;
using ToDoList.Core.Repositories;
using Entities;
using ToDoList.Infrastructure.Mapping;
using ToDoList.Core.Enums;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ToDoList.Core.ServiceContracts;
using ToDoList.Core.Constants;




namespace Services
{
    public class ToDoItemsService : IToDoItemsService 
    {


        private readonly IToDoItemRepository _repository;
        private readonly IMappingService _mapper;
        private readonly ICurrentUserService _currentUserService;


        public ToDoItemsService(IToDoItemRepository repository, IMappingService mapper, ICurrentUserService currentUserService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }


        public async Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId)
        {
            if (todoItemId == null)
                return null;

            var userId = _currentUserService.GetUserId();
            if (userId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);

            var result = await _repository.GetByIdAsync(todoItemId, userId.Value);

            return _mapper.Map<TodoItem, ToDoItemResponse>(result);
        }

        public async Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? request)
        {

            if (request == null)
                throw new ArgumentNullException(nameof(request));

            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UserNotAuthenticated);

            var isAdmin = _currentUserService.IsInRole("Admin");
            var effectiveUserId = isAdmin && request.UserId.HasValue
                ? request.UserId.Value
                : currentUserId.Value;

            var item = await _repository.GetByIdAsync(request.Id);
            if (item == null || (!isAdmin && item.UserId != effectiveUserId))
                throw new ArgumentException(ErrorMessages.TodoItemNotFoundOrAccessDenied);

            if (item.Status == TodoStatus.Completed)
                throw new InvalidOperationException(ErrorMessages.CannotUpdateCompletedTask);

            item.Title = request.Title!;
            item.Description = request.Description;
            item.Status = request.Status;
            item.DueDate = request.DueDate!.Value;

            await _repository.SaveChangesAsync();
            return _mapper.Map<TodoItem, ToDoItemResponse>(item);

        }


  
        public async Task<bool> DeleteTodoItemAsync(Guid? todoItemId)
        {
            if (todoItemId == Guid.Empty)
                throw new ArgumentException(ErrorMessages.TodoItemIdIsRequired, nameof(todoItemId));

            var currentUserId = _currentUserService.GetUserId();
            if (currentUserId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UserNotAuthenticated);

            var isAdmin = _currentUserService.IsInRole("Admin");

            return await _repository.DeleteAsync(todoItemId, currentUserId.Value, isAdmin);
        }



        public async Task<List<ToDoItemResponse>> AddTodoItemsAsync(
     List<TodoItemAddRequest> requestList)
        {

            var tokenUserId = _currentUserService.GetUserId();
            if (tokenUserId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);

            bool isAdmin = _currentUserService.IsInRole("Admin");


            var entities = requestList.Select(request =>
            {
                var entity = _mapper.Map<TodoItemAddRequest, TodoItem>(request);
                entity.UserId = isAdmin && request.UserId.HasValue
                    ? request.UserId.Value
                    : tokenUserId.Value;
                entity.CreationDate = DateTime.UtcNow;
                return entity;
            }).ToList();

            var result = new List<TodoItem>();

            foreach (var item in entities)
            {
                int activeCount = await _repository.CountActiveAsync(item.UserId);
                if (activeCount >= 10)
                    throw new InvalidOperationException(ErrorMessages.UserHasMaxActiveItems);

                await _repository.AddAsync(item);
                result.Add(item);
            }

            await _repository.SaveChangesAsync();

            return _mapper.MapList<TodoItem, ToDoItemResponse>(result);
        }



        public async Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync()
        {
            bool isAdmin = _currentUserService.IsInRole("Admin");
            if (isAdmin == false)
                throw new UnauthorizedAccessException(ErrorMessages.AdminsOnly);

           

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

            bool isAdmin = _currentUserService.IsInRole("Admin");
            if (isAdmin == false)
                throw new UnauthorizedAccessException(ErrorMessages.AdminsOnly);

            return await _repository.RestoreAsync(todoItemId);
        }


        /// <summary>
        /// Retrieves a single soft-deleted todo item by its ID by ignoring global query filters.
        /// </summary>
        /// <param name="todoItemId">The ID of the soft-deleted todo item to retrieve.</param>
        /// <returns>The soft-deleted todo item mapped to a response model, or null if not found.</returns>
        public async Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId)
        {


            bool isAdmin = _currentUserService.IsInRole("Admin");
            if (isAdmin == false)
                throw new UnauthorizedAccessException(ErrorMessages.AdminsOnly);

            TodoItem? todoItem =  await _repository.GetDeletedItemByIdAsync(todoItemId);

            if (todoItem == null) return null;


            return _mapper.Map<TodoItem, ToDoItemResponse>(todoItem);

        }



       
        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request)
        {
 
            
            List<TodoItem> todoItems =  await _repository.GetPaginatedAsync(request);
            return _mapper.MapList<TodoItem, ToDoItemResponse>(todoItems);

        }


        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync()
        {

            var userId =  _currentUserService.GetUserId();

            if (userId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);


            List<TodoItem> result = await _repository.GetAllByUserAsync(userId.Value);
            return  _mapper.MapList<TodoItem, ToDoItemResponse>(result);

        }


     
        public async Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync()
        {
            if (!_currentUserService.IsInRole("Admin"))
                throw new UnauthorizedAccessException(ErrorMessages.AdminsOnly);

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

        public async Task MarkAsInProgressAsync(List<Guid> itemIds)
        {

            var currentUserId = _currentUserService.GetUserId();
           
            if (currentUserId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);

            bool isAdmin = _currentUserService.IsInRole("Admin");

            var items = await _repository.ListByIdsAsync(itemIds);


            if (!isAdmin)   
            {
                if (items.Any(t => t.UserId != currentUserId))
                    throw new UnauthorizedAccessException(ErrorMessages.ModifyOwnItemsOnly);
            }

            foreach (var item in items)
            {
                if (item.Status == TodoStatus.Completed)
                    throw new InvalidOperationException(ErrorMessages.InvalidItemStatus);

                item.Status = TodoStatus.InProgress;

            }

            await _repository.SaveChangesAsync(); 
        }


        public async Task MarkAsCompletedAsync(List<Guid> itemIds)
        {

            var currentUserId = _currentUserService.GetUserId();

            if (currentUserId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);

            bool isAdmin = _currentUserService.IsInRole("Admin");


            var items = await _repository.ListByIdsAsync(itemIds);

            if (!isAdmin && items.Any(t => t.UserId != currentUserId))
                throw new UnauthorizedAccessException(ErrorMessages.ModifyOwnItemsOnly);

            foreach (var item in items)
            {
                item.Status = TodoStatus.Completed;
            }

            await _repository.SaveChangesAsync();
        }

        public async Task MarkAsNewAsync(List<Guid> itemIds)
        {


            var currentUserId = _currentUserService.GetUserId();

            if (currentUserId == null)
                throw new UnauthorizedAccessException(ErrorMessages.UnauthorizedUser);

            bool isAdmin = _currentUserService.IsInRole("Admin");


            var items = await _repository.ListByIdsAsync(itemIds);

            // Only allow updating items that belong to the current user
            if (items.Any(t => t.UserId != currentUserId))
                throw new UnauthorizedAccessException(ErrorMessages.ModifyOwnItemsOnly);

            foreach (var item in items)
            {
                if (item.Status == TodoStatus.Completed)
                    throw new InvalidOperationException(ErrorMessages.InvalidItemStatus);

                // Only update items that are currently InProgress
                if (item.Status == TodoStatus.InProgress)
                {
                    item.Status = TodoStatus.New;
                }
            }

            await _repository.SaveChangesAsync();
        }


        public async Task ResetCompletedToInProgressAsync(List<Guid> itemIds)
        {
            bool isAdmin = _currentUserService.IsInRole(UserRoles.Admin);

            if (!isAdmin)
                throw new UnauthorizedAccessException(ErrorMessages.AdminOnlyAction);

            var items = await _repository.ListByIdsAsync(itemIds);

            foreach (var item in items)
            {
                // Only update items that are Completed
                if (item.Status == TodoStatus.Completed)
                {
                    item.Status = TodoStatus.InProgress;
                }
                else
                {
                    throw new InvalidOperationException(ErrorMessages.CannotResetUnlessCompleted);
                }

            }

            await _repository.SaveChangesAsync();
        }





    }
}
