using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts.DTO;
using ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Services;
using ToDoList.Core.DTO;
using Entities;
using Mapster;

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
        public async Task<IActionResult> AddTodoItem([FromBody] List<TodoItemAddRequest> requestList)
        {
            if (requestList == null || requestList.Count == 0)
                return BadRequest();

            var createdResponses = await _todoItemsService.AddTodoItemsAsync(requestList);

            return Created("", createdResponses);
        }





        // GET: api/todoitems/{todoItemId}
        [HttpGet("{todoItemId}")]
        public async Task<IActionResult> GetTodoItemById(Guid todoItemId)
        {
            try
            {
                var item = await _todoItemsService.GetTodoItemByIdAsync(todoItemId);
                if (item == null)
                    return NotFound();

                return Ok(item);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
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
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTodoItem(Guid id, [FromBody] ToDoItemUpdateRequest request)
        {
            if (request == null) return BadRequest();

            // Extract userId from the JWT token using ClaimTypes.NameIdentifier
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

            if (userIdClaim == null) return Unauthorized("User ID claim not found.");


            var userId = Guid.Parse(userIdClaim.Value); 

            var isAdmin = User.IsInRole("Admin");
            var actualUserId = isAdmin && request.UserId.HasValue ? request.UserId.Value : userId;

            // Set the TodoItem Id from the route parameter
            request.Id = id;

            
            var result = await _todoItemsService.UpdateTodoItemAsync(request, actualUserId, isAdmin);

            return Ok(result);
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
        public async Task<IActionResult> GetPaginatedTodoItems([FromQuery] PaginationRequest request)
        {
            if (request.PageNumber <= 0 || request.PageSize <= 0)
                return BadRequest("Page number and page size must be greater than 0.");

            var paginatedItems = await _todoItemsService.GetPaginatedItemsAsync(request);
            return Ok(paginatedItems);
        }


        [HttpPut("status/in-progress")]
        public async Task<IActionResult> SetItemsToInProgress([FromBody] UpdateTodoStatusRequest request)
        {
            if (request == null || request.TodoItemIds == null || !request.TodoItemIds.Any())
                return BadRequest("No IDs provided.");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid tokenUserId))
                return Unauthorized("Invalid or missing user ID claim.");

            bool isAdmin = User.IsInRole("Admin");

            try
            {
                await _todoItemsService.MarkAsInProgressAsync(request.TodoItemIds, tokenUserId, isAdmin);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }



        [HttpPut("status/in-completed")]
        public async Task<IActionResult> SetItemsToCompleted([FromBody] UpdateTodoStatusRequest request)
        {
            if (request == null || request.TodoItemIds == null || !request.TodoItemIds.Any())
                return BadRequest("No IDs provided.");

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid tokenUserId))
                return Unauthorized("Invalid or missing user ID claim.");
            //use http accessor to list user claims
            bool isAdmin = User.IsInRole("Admin");

            try
            {
                await _todoItemsService.MarkAsCompletedAsync(request.TodoItemIds, tokenUserId, isAdmin);
                return NoContent();
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize]
        [HttpPut("status/new")]
        public async Task<IActionResult> MarkItemsAsNew([FromBody] UpdateTodoStatusRequest request)
        {
            if (request == null || request.TodoItemIds == null || !request.TodoItemIds.Any())
                return BadRequest("No item IDs provided.");

            // Extract current user's ID from claims
            
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid currentUserId))
                return Unauthorized("Invalid or missing user ID claim.");

            try
            {
                await _todoItemsService.MarkAsNewAsync(request.TodoItemIds, currentUserId);
                return NoContent(); // 204
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("status/reset-to-inprogress")]
        public async Task<IActionResult> ResetCompletedItemsToInProgress([FromBody] UpdateTodoStatusRequest request)
        {
            if (request == null || request.TodoItemIds == null || !request.TodoItemIds.Any())
                return BadRequest("No item IDs provided.");

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !Guid.TryParse(userIdClaim.Value, out Guid currentUserId))
                return Unauthorized("Invalid or missing user ID claim.");

            try
            {
                await _todoItemsService.ResetCompletedToInProgressAsync(request.TodoItemIds, currentUserId, isAdmin: true);
                return NoContent(); // 204
            }
            catch (UnauthorizedAccessException)
            {
                return Forbid();    
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }











    }
}