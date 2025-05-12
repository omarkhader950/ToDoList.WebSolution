using ToDoList.Core.DTO;
using ServiceContracts;
using ServiceContracts.DTO;
using ToDoList.Core.Repositories;
using Entities;
using ToDoList.Infrastructure.Mapping;




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

            return await _repository.GetTodoItemByIdAsync(todoItemId,userId); 
        }

        public async Task<ToDoItemResponse> UpdateTodoItemAsync(ToDoItemUpdateRequest? todoItemUpdateRequest, Guid actualUserId, bool isAdmin)
        {
   

            return await _repository.UpdateTodoItemAsync(todoItemUpdateRequest,actualUserId,isAdmin);
        }

        public async Task<bool> DeleteTodoItemAsync(Guid? todoItemId, Guid tokenUserId, bool isAdmin)
        {
            return await _repository.DeleteTodoItemAsync(todoItemId, tokenUserId, isAdmin);
        }



        public async Task<List<TodoItem>> AddTodoItemsAsync(List<TodoItem> items)
        {
            var result = new List<TodoItem>();

            foreach (var item in items)
            {
                int activeCount = await _repository.CountActiveAsync(item.UserId);
                if (activeCount >= 10)
                    throw new InvalidOperationException("User already has 10 active ToDo items.");

                await _repository.AddAsync(item);
                result.Add(item);
            }

            await _repository.SaveChangesAsync();
            return result;
        }


        /// <summary>
        /// Retrieves all soft-deleted todo items by ignoring global query filters.
        /// </summary>
        /// <returns>List of deleted todo items mapped to response models.</returns>
        public async Task<List<ToDoItemResponse>> GetAllDeletedItemsAsync()
        {
            return await _repository.GetAllDeletedItemsAsync();


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
           

            return await _repository.GetDeletedItemByIdAsync(todoItemId);
        }



       
        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(PaginationRequest request)
        {
            return await _repository.GetPaginatedItemsAsync(request);

        }


        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId)
        {
            return await _repository.GetAllTodoItemsByUserAsync(userId);
        }



        public async Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync()
        {
            return await _repository.GetAllTodoItemsGroupedByUserAsync();
        }

        public async Task MarkAsInProgressAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin)
        {


           await _repository.MarkAsInProgressAsync(itemIds, currentUserId, isAdmin);

           
        }

        public async Task MarkAsCompletedAsync(List<Guid> itemIds, Guid currentUserId, bool isAdmin)
        {


            await _repository.MarkAsCompletedAsync(itemIds, currentUserId, isAdmin);


        }




    }
}
