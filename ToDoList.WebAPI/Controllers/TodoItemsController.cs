using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Services;
using ToDoList.Core.DTO;

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




        // admin and user in the same end point 
        // POST: api/todoitems
        [HttpPost]
        public async Task<IActionResult> AddTodoItem([FromBody] TodoItemAddRequest request)
        {
            if (request == null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid tokenUserId))
                return Unauthorized("Invalid or missing user ID claim.");

            // Check if the current user is an Admin
            bool isAdmin = User.IsInRole("Admin");

            // Decide which userId to assign:
            Guid finalUserId = isAdmin && request.UserId.HasValue
                ? request.UserId.Value    // Admin provides it
                : tokenUserId;            // Regular user gets it from token

            var createdItem = await _todoItemsService.AddTodoItemAsync(request, finalUserId);
            return CreatedAtAction(nameof(GetTodoItemById), new { todoItemId = createdItem.Id }, createdItem);
        }

     

        // GET: api/todoitems/{todoItemId}
        [HttpGet("{todoItemId}")]
        public async Task<IActionResult> GetTodoItemById(Guid todoItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            var item = await _todoItemsService.GetTodoItemByIdAsync(todoItemId, userId);
            if (item == null) return NotFound();

            return Ok(item);
        }

        // GET: api/todoitems/by-user
        [HttpGet("by-user")]
        public async Task<IActionResult> GetTodoItemsByUser()
        {
            // Extract user ID from the JWT token claims
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            // Get the items by user
            var userItems = await _todoItemsService.GetAllTodoItemsByUserAsync(userId);
            return Ok(userItems);
        }


        [Authorize(Roles = "Admin")]
        // GET: api/todoitems
        [HttpGet]
        public async Task<ActionResult<List<UserWithTodoItemsResponse>>> GetAllGroupedByUser()
        {
            var result = await _todoItemsService.GetAllTodoItemsGroupedByUserAsync();

            return Ok(result);
        }
        // PUT: api/todoitems
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTodoItem([FromBody] ToDoItemUpdateRequest request)
        {
            if (request == null) return BadRequest();

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid userId))
                return Unauthorized("Invalid or missing user ID claim.");

            var updated = await _todoItemsService.UpdateTodoItemAsync(request, userId);
            return Ok(updated);
        }


        
        // DELETE: api/todoitems/{todoItemId}
        [HttpDelete("{todoItemId}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteTodoItem(Guid todoItemId)
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid tokenUserId))
                return Unauthorized("Invalid or missing user ID claim.");

            bool isAdmin = User.IsInRole("Admin");

            var success = await _todoItemsService.DeleteTodoItemAsync(todoItemId, tokenUserId, isAdmin);
            if (!success) return NotFound();

            return NoContent();
        }


        [Authorize(Roles = "Admin")]
        // GET: api/todoitems/deleted
        [HttpGet("deleted")]
        public async Task<ActionResult<List<ToDoItemResponse>>> GetDeletedTodoItems()
        {
            var deletedItems = await _todoItemsService.GetAllDeletedItemsAsync();
            return Ok(deletedItems);
        }




        [Authorize(Roles ="Admin")]
        // GET: api/todoitems/deleted/{todoItemId}
        [HttpGet("deleted/{todoItemId}")]
        public async Task<ActionResult<ToDoItemResponse>> GetDeletedTodoItemById(Guid todoItemId)
        {
            var item = await _todoItemsService.GetDeletedItemByIdAsync(todoItemId);

            if (item == null)
                return NotFound();

            return Ok(item);
        }

        [Authorize(Roles = "Admin")]
        // PATCH: api/todoitems/restore/{todoItemId}
        [HttpPatch("restore/{todoItemId}")]
        public async Task<IActionResult> RestoreTodoItem(Guid todoItemId)
        {
            var restored = await _todoItemsService.RestoreTodoItemAsync(todoItemId);
            if (!restored)
                return NotFound();

            return NoContent();
        }



       
        // GET: api/todoitems/paginated
        [HttpGet("paginated")]
        public async Task<IActionResult> GetPaginatedTodoItems([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 3)
        {
            if (pageNumber <= 0 || pageSize <= 0)
                return BadRequest("Page number and page size must be greater than 0.");

            var paginatedItems = await _todoItemsService.GetPaginatedItemsAsync(pageNumber, pageSize);
            return Ok(paginatedItems);
        }

       


       





    }
}