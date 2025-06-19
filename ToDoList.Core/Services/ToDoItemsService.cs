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
using ToDoList.Core.DTO.AttachmentDto;
using ToDoList.Core.Entities.Entities;




namespace Services
{
    public class ToDoItemsService : IToDoItemsService
    {


        private readonly IToDoItemRepository _repository;
        private readonly IMappingService _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IValidationService _validationService;


        public ToDoItemsService(IToDoItemRepository repository, IMappingService mapper, ICurrentUserService currentUserService, IValidationService validationService)
        {
            _repository = repository;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _validationService = validationService;
        }


        public async Task<ToDoItemResponse?> GetTodoItemByIdAsync(Guid? todoItemId)
        {
            _validationService.EnsureNotNull(todoItemId, nameof(todoItemId));


            var userId = _currentUserService.GetUserId();

            _validationService.EnsureUserIsAuthenticated(userId);



            var result = await _repository.GetByIdAsync(todoItemId, userId.Value);

            return _mapper.Map<TodoItem, ToDoItemResponse>(result);
        }

        public async Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? request)
        {

            _validationService.EnsureNotNull(request, nameof(request));


            var currentUserId = _currentUserService.GetUserId();

            _validationService.EnsureUserIsAuthenticated(currentUserId);

            var isAdmin = _currentUserService.IsAdmin();

            _validationService.ValidateUserIsAdminOrOwnsResource(isAdmin, request.UserId, currentUserId);


            var effectiveUserId = isAdmin && request.UserId.HasValue
                ? request.UserId.Value
                : currentUserId.Value;

            var item = await _repository.GetByIdAsync(request.Id);

            _validationService.ValidateUserOwnsResource(item, effectiveUserId, isAdmin);


            _validationService.ValidateTodoItemStatusForUpdate(item.Status);


            item.Title = request.Title!;
            item.Description = request.Description;
            item.Status = request.Status;
            item.DueDate = request.DueDate!.Value;

            await _repository.SaveChangesAsync();
            return _mapper.Map<TodoItem, ToDoItemResponse>(item);

        }



        public async Task<bool> DeleteTodoItemAsync(Guid? todoItemId)
        {


            var currentUserId = _currentUserService.GetUserId();

            _validationService.EnsureUserIsAuthenticated(currentUserId);


            var isAdmin = _currentUserService.IsAdmin();

            return await _repository.DeleteAsync(todoItemId, currentUserId.Value, isAdmin);
        }



        public async Task<List<ToDoItemResponse>> AddTodoItemsAsync(
     List<TodoItemAddRequest> requestList)
        {

            var userId = _currentUserService.GetUserId();
            _validationService.EnsureUserIsAuthenticated(userId);

            bool isAdmin = _currentUserService.IsAdmin();


            var entities = requestList.Select(request =>
            {
                var entity = _mapper.Map<TodoItemAddRequest, TodoItem>(request);
                entity.UserId = isAdmin && request.UserId.HasValue
                    ? request.UserId.Value
                    : userId.Value;
                entity.CreationDate = DateTime.UtcNow;
                return entity;
            }).ToList();

            var result = new List<TodoItem>();

            foreach (var item in entities)
            {
                int activeCount = await _repository.CountActiveAsync(item.UserId);
                _validationService.ValidateMaxActiveItemsLimit(activeCount);

                await _repository.AddAsync(item);
                result.Add(item);
            }

            await _repository.SaveChangesAsync();

            return _mapper.MapList<TodoItem, ToDoItemResponse>(result);
        }



        public async Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync()
        {
            bool isAdmin = _currentUserService.IsAdmin();


            _validationService.ValidateUserIsAdmin(isAdmin);

            List<TodoItem> todoItems = await _repository.GetAllDeletedItemsAsync();

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

            bool isAdmin = _currentUserService.IsAdmin();

            _validationService.ValidateUserIsAdmin(isAdmin);


            return await _repository.RestoreAsync(todoItemId);
        }


        /// <summary>
        /// Retrieves a single soft-deleted todo item by its ID by ignoring global query filters.
        /// </summary>
        /// <param name="todoItemId">The ID of the soft-deleted todo item to retrieve.</param>
        /// <returns>The soft-deleted todo item mapped to a response model, or null if not found.</returns>
        public async Task<ToDoItemResponse?> GetDeletedItemByIdAsync(Guid todoItemId)
        {


            bool isAdmin = _currentUserService.IsAdmin();

            _validationService.ValidateUserIsAdmin(isAdmin);


            TodoItem? todoItem = await _repository.GetDeletedItemByIdAsync(todoItemId);

            if (todoItem == null) return null;


            return _mapper.Map<TodoItem, ToDoItemResponse>(todoItem);

        }




        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request)
        {


            List<TodoItem> todoItems = await _repository.GetPaginatedAsync(request);
            return _mapper.MapList<TodoItem, ToDoItemResponse>(todoItems);

        }


        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync()
        {

            var userId = _currentUserService.GetUserId();

            _validationService.EnsureUserIsAuthenticated(userId);


            List<TodoItem> result = await _repository.GetAllByUserAsync(userId.Value);
            return _mapper.MapList<TodoItem, ToDoItemResponse>(result);

        }



        public async Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync()
        {

            bool isAdmin = _currentUserService.IsAdmin();

            _validationService.ValidateUserIsAdmin(isAdmin);

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

            _validationService.EnsureUserIsAuthenticated(currentUserId);


            bool isAdmin = _currentUserService.IsAdmin();

            var items = await _repository.ListByIdsAsync(itemIds);



            foreach (var item in items)
            {
                _validationService.ValidateUserOwnsResource(item, currentUserId.Value, isAdmin);

                _validationService.ValidateTodoItemStatusForUpdate(item.Status);


                item.Status = TodoStatus.InProgress;

            }


            await _repository.SaveChangesAsync();
        }


        public async Task MarkAsCompletedAsync(List<Guid> itemIds)
        {

            var currentUserId = _currentUserService.GetUserId();

            _validationService.EnsureUserIsAuthenticated(currentUserId);


            bool isAdmin = _currentUserService.IsAdmin();


            var items = await _repository.ListByIdsAsync(itemIds);



            foreach (var item in items)
            {
                _validationService.ValidateUserOwnsResource(item, currentUserId.Value, isAdmin);

                item.Status = TodoStatus.Completed;
            }

            await _repository.SaveChangesAsync();
        }

        public async Task MarkAsNewAsync(List<Guid> itemIds)
        {


            var currentUserId = _currentUserService.GetUserId();

            _validationService.EnsureUserIsAuthenticated(currentUserId);


            bool isAdmin = _currentUserService.IsAdmin();


            var items = await _repository.ListByIdsAsync(itemIds);





            foreach (var item in items)
            {

                _validationService.ValidateUserOwnsResource(item, currentUserId.Value);
                _validationService.ValidateTodoItemStatusForUpdate(item.Status);


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

            bool isAdmin = _currentUserService.IsAdmin();
            _validationService.ValidateUserIsAdmin(isAdmin);


            var items = await _repository.ListByIdsAsync(itemIds);

            _validationService.ValidateAllItemsAreCompleted(items);

            foreach (var item in items)
            {

                item.Status = TodoStatus.InProgress;

            }

            await _repository.SaveChangesAsync();
        }

        public async Task<TodoItemAttachmentResponse> UploadAttachmentAsync(Guid todoItemId, IFormFile file)
        {
            _validationService.EnsureNotNull(file, nameof(file));

            var userId = _currentUserService.GetUserId();
            _validationService.EnsureUserIsAuthenticated(userId);

            var todoItem = await _repository.GetByIdAsync(todoItemId, userId.Value);
            _validationService.EnsureNotNull(todoItem, nameof(todoItem));

            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Attachments");
            Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = $"/Attachments/{uniqueFileName}";

            var attachment = new TodoItemAttachment
            {
                TodoItemId = todoItemId,
                FileName = file.FileName,
                FilePath = relativePath,
                UploadedAt = DateTime.UtcNow,
                UploadedBy = userId.ToString()
            };

            await _repository.AddAttachmentAsync(attachment); // TODO ::You need to define this
            await _repository.SaveChangesAsync();

            return new TodoItemAttachmentResponse
            {
                Id = attachment.Id,
                TodoItemId = attachment.TodoItemId,
                FileName = attachment.FileName,
                FilePath = attachment.FilePath,
                UploadedAt = attachment.UploadedAt,
                UploadedBy = attachment.UploadedBy
            };
        }
    }
}
