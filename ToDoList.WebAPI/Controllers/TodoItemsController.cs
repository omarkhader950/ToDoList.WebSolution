using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts;
using Microsoft.AspNetCore.Authorization;

namespace ToDoList.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TodoItemsController : ControllerBase
    {
        private readonly IToDoItemsService _todoItemsService;

        public TodoItemsController(IToDoItemsService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        // POST: api/todoitems
        [HttpPost]
        public IActionResult AddTodoItem([FromBody] TodoItemAddRequest request)
        {
            if (request == null) return BadRequest();

            var createdItem = _todoItemsService.AddTodoItem(request);
            return CreatedAtAction(nameof(GetTodoItemById), new { todoItemId = createdItem.Id }, createdItem);
        }

        // GET: api/todoitems
        [HttpGet]
        public IActionResult GetAllTodoItems()
        {
            var items = _todoItemsService.GetAllTodoItems();
            return Ok(items);
        }

        // GET: api/todoitems/{id}
        [HttpGet("{todoItemId}")]
        public IActionResult GetTodoItemById(Guid todoItemId)
        {
            var item = _todoItemsService.GetTodoItemById(todoItemId);
            if (item == null) return NotFound();

            return Ok(item);
        }

        // PUT: api/todoitems
        [HttpPut]
        public IActionResult UpdateTodoItem([FromBody] ToDoItemUpdateRequest request)
        {
            if (request == null) return BadRequest();

            var updated = _todoItemsService.UpdateTodoItem(request);
            return Ok(updated);
        }

        // DELETE: api/todoitems/{id}
        [HttpDelete("{todoItemId}")]
        public IActionResult DeleteTodoItem(Guid todoItemId)
        {
            var success = _todoItemsService.DeleteTodoItem(todoItemId);
            if (!success) return NotFound();

            return NoContent();
        }

        // GET: api/todoitems/deleted
        [HttpGet("deleted")]
        public ActionResult<List<ToDoItemResponse>> GetDeletedTodoItems()
        {
            var deletedItems = _todoItemsService.GetAllDeletedItems();
            return Ok(deletedItems);
        }

        // PATCH: api/todoitems/restore/{todoItemId}
        [HttpPatch("restore/{todoItemId}")]
        public IActionResult RestoreTodoItem(Guid todoItemId)
        {
            var restored = _todoItemsService.RestoreTodoItem(todoItemId);
            if (!restored)
                return NotFound();

            return NoContent();
        }


        // GET: api/todoitems/deleted/{todoItemId}
        [HttpGet("deleted/{todoItemId}")]
        public ActionResult<ToDoItemResponse> GetDeletedTodoItemById(Guid todoItemId)
        {
            var item = _todoItemsService.GetDeletedItemById(todoItemId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

       
        // GET: api/todoitems/paginated
        [HttpGet("paginated")]
        public IActionResult GetPaginatedTodoItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than 0.");

            var paginatedItems = _todoItemsService.GetPaginatedItems(pageNumber, pageSize);
            return Ok(paginatedItems);
        }





    }
}