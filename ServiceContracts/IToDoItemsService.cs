using Entities;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceContracts
{
    public interface IToDoItemsService
    {
        /// <summary>
        /// Adds a new task to the list of TodoItems
        /// </summary>
        /// <param name="todoItemAddRequest">Task to add</param>
        /// <returns>Returns the task details along with the newly generated ID</returns>
        ToDoItemResponse AddTodoItem(TodoItemAddRequest? todoItemAddRequest);

        /// <summary>
        /// Returns all TodoItems
        /// </summary>
        /// <returns>Returns a list of TodoItemResponse objects</returns>
        List<ToDoItemResponse> GetAllTodoItems();

        /// <summary>
        /// Returns the TodoItem object based on the given ID
        /// </summary>
        /// <param name="todoItemId">ID of the task to retrieve</param>
        /// <returns>Returns matching task as TodoItemResponse</returns>
        ToDoItemResponse? GetTodoItemById(Guid? todoItemId);

        /// <summary>
        /// Returns filtered list of TodoItems by title or description
        /// </summary>
        /// <param name="searchBy">Search field (title/description)</param>
        /// <param name="searchString">Search value</param>
        /// <returns>Returns list of matching TodoItemResponse objects</returns>
       // List<ToDoItemResponse> GetFilteredTodoItems(string searchBy, string? searchString);

        /// <summary>
        /// Returns sorted TodoItems list
        /// </summary>
        /// <param name="allItems">List of TodoItems to sort</param>
        /// <param name="sortBy">Name of the property to sort by</param>
        /// <param name="sortOrder">Sort direction (ASC/DESC)</param>
        /// <returns>Returns sorted list of TodoItemResponse objects</returns>
       
        //List<ToDoItemResponse> GetSortedTodoItems(List<ToDoItemResponse> allItems, string sortBy, SortOrderOptions sortOrder);

        /// <summary>
        /// Updates the specified TodoItem based on its ID
        /// </summary>
        /// <param name="todoItemUpdateRequest">Todo item data to update</param>
        /// <returns>Returns updated TodoItemResponse object</returns>
        ToDoItemResponse UpdateTodoItem(ToDoItemUpdateRequest? todoItemUpdateRequest);

        /// <summary>
        /// Deletes a TodoItem by its ID
        /// </summary>
        /// <param name="todoItemId">ID of the task to delete</param>
        /// <returns>Returns true if deletion is successful, otherwise false</returns>
        bool DeleteTodoItem(Guid? todoItemId);



        List<ToDoItemResponse> GetAllDeletedItems();



        bool RestoreTodoItem(Guid todoItemId);


        public ToDoItemResponse? GetDeletedItemById(Guid todoItemId);





        public List<ToDoItemResponse> GetPaginatedItems(int pageNumber, int pageSize);




        public List<ToDoItemResponse> GetAllTodoItemsByUser(Guid userId);




    }
}
