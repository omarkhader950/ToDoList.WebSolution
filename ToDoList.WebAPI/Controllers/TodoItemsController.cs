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
using ToDoList.Core.Constants;

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
                return BadRequest(ErrorMessages.EmptyTodoItemList);

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
        
            var userItems = await _todoItemsService.GetAllTodoItemsByUserAsync();
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
        public async Task<IActionResult> UpdateTodoItem(Guid id, [FromBody] ToDoItemUpdateRequest request)
        {
            if (request == null) return BadRequest();

            request.Id = id;

            
            var result = await _todoItemsService.UpdateTodoItemAsync(request);

            return Ok(result);
        }

        
        
        // DELETE: api/todoitems/{todoItemId}
        [HttpDelete("{todoItemId}")]
        public async Task<IActionResult> DeleteTodoItem(Guid todoItemId)
        {
            var deleted = await _todoItemsService.DeleteTodoItemAsync(todoItemId);
            if (!deleted) return NotFound();

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
            if (request.PageNumber <= ValidationConstants.MinPageNumber || request.PageSize <= ValidationConstants.MinPageSize)
                return BadRequest(ErrorMessages.InvalidPagination);

            var paginatedItems = await _todoItemsService.GetPaginatedItemsAsync(request);
            return Ok(paginatedItems);
        }


        [HttpPut("status/in-progress")]
        public async Task<IActionResult> SetItemsToInProgress([FromBody] UpdateTodoStatusRequest request)
        {
            if (request == null || request.TodoItemIds == null || !request.TodoItemIds.Any())
                return BadRequest("No IDs provided.");

       
            try
            {
                await _todoItemsService.MarkAsInProgressAsync(request.TodoItemIds);
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

           

            try
            {
                await _todoItemsService.MarkAsCompletedAsync(request.TodoItemIds);
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

            try
            {
                await _todoItemsService.MarkAsNewAsync(request.TodoItemIds);
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

           

            try
            {
                await _todoItemsService.ResetCompletedToInProgressAsync(request.TodoItemIds);
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

        [Authorize]
        [HttpPost("{todoItemId}/attachments")]
        public async Task<IActionResult> UploadAttachment(Guid todoItemId, IFormFile file)
        {

            var result = await _todoItemsService.UploadAttachmentAsync(todoItemId, file);
            return Ok(result);

        }













    }
}