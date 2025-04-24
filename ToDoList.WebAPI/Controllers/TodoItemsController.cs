using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Services;

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

        [Authorize(Roles = "Admin")]
        // GET: api/todoitems
        //[HttpGet]
        //public IActionResult GetAllTodoItems()
        //{
        //    var items = _todoItemsService.GetAllTodoItems();
        //    return Ok(items);
        //}

        // GET: api/todoitems/{todoItemId}
        [HttpGet("{todoItemId}")]
        public IActionResult GetTodoItemById(Guid todoItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            


            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            var item = _todoItemsService.GetTodoItemById(todoItemId, userId);
            if (item == null) return NotFound();

            return Ok(item);
        }

       
        // PUT: api/todoitems
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public IActionResult UpdateTodoItem([FromBody] ToDoItemUpdateRequest request)
        {
            if (request == null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            var updated = _todoItemsService.UpdateTodoItem(request, userId);
            return Ok(updated);
        }


        [Authorize(Roles = "Admin")]
        // DELETE: api/todoitems/{todoItemId}
        [HttpDelete("{todoItemId}")]
        public IActionResult DeleteTodoItem(Guid todoItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            var success = _todoItemsService.DeleteTodoItem(todoItemId, userId);
            if (!success) return NotFound();

            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        // GET: api/todoitems/deleted
        [HttpGet("deleted")]
        public ActionResult<List<ToDoItemResponse>> GetDeletedTodoItems()
        {
            var deletedItems = _todoItemsService.GetAllDeletedItems();
            return Ok(deletedItems);
        }


        [Authorize(Roles = "Admin")]
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

        // GET: api/todoitems/by-user
        [HttpGet("by-user")]
        public IActionResult GetTodoItemsByUser()
        {
            // Extract user ID from the JWT token claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            // Get the items by user
            var userItems = _todoItemsService.GetAllTodoItemsByUser(userId);
            return Ok(userItems);
        }


        // GET: api/todoitems/admin/by-user/{userId}
        [HttpGet("admin/by-user/{userId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetTodoItemsByUserIdAsAdmin(Guid userId)
        {
            var items = _todoItemsService.GetAllTodoItemsByUser(userId);
            return Ok(items);
        }


        [Authorize(Roles = "Admin")]
        // GET: api/todoitems
        [HttpGet]
        public ActionResult<List<UserWithTodoItemsResponse>> GetAllGroupedByUser()
        {
            var result = _todoItemsService.GetAllTodoItemsGroupedByUser();

            return Ok(result);
        }



    }
}