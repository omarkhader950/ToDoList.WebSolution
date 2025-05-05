using ToDoList.Core.DTO;
using ServiceContracts;
using ServiceContracts.DTO;
using ToDoList.Core.Repositories;




namespace Services
{
    public class ToDoItemsService : IToDoItemsService
    {


        private readonly IToDoItemRepository _repository;
       


     


        public ToDoItemsService(IToDoItemRepository repository)
        {
            _repository = repository;
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



        public async Task<ToDoItemResponse> AddTodoItemAsync(TodoItemAddRequest? todoItemAddRequest, Guid userId)
        {

            return  await _repository.AddTodoItemAsync(todoItemAddRequest, userId); 
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



       
        public async Task<List<ToDoItemResponse>> GetPaginatedItemsAsync(int pageNumber, int pageSize)
        {
            return await _repository.GetPaginatedItemsAsync(pageNumber,pageSize);

        }

        //public async Task<List<ToDoItemResponse>> GetPaginatedItemsForUserAsync(Guid userId, int pageNumber, int pageSize)
        //{
        //    return await _db.TodoItems
        //      .Include(t => t.User)
        //      .Where(t => t.UserId == userId && t.DeleteBy == null) // Active items only
        //      .OrderBy(t => t.Id)
        //      .Skip((pageNumber - 1) * pageSize)
        //      .Take(pageSize)
        //      .Select(t => t.Adapt<ToDoItemResponse>())
        //      .ToListAsync();
        //}



        public async Task<List<ToDoItemResponse>> GetAllTodoItemsByUserAsync(Guid userId)
        {
            return await _repository.GetAllTodoItemsByUserAsync(userId);
        }



        public async Task<List<UserWithTodoItemsResponse>> GetAllTodoItemsGroupedByUserAsync()
        {
            return await _repository.GetAllTodoItemsGroupedByUserAsync();
        }

        public Task<List<ToDoItemResponse>> GetPaginatedItemsForUserAsync(Guid userId, int pageNumber, int pageSize)
        {
            throw new NotImplementedException();
        }

       
    }
}
