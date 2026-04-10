using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.UserBooks.Commands;
using ShelfManager.Application.Handlers.UserBooks.Queries;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBookController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public UserBookController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("{bookId}")]
        public async Task<IActionResult> BorrowBook( [FromRoute] Guid bookId)
        {
            var result = await _mediator.Send(new BorrowBookCommandRequest { BookId = bookId });
            return Ok(result);
        }

        [HttpPut("{id}/return")]
        public async Task<IActionResult> ReturnBook([FromRoute] Guid id, [FromBody] ReturnBookCommandRequest request)
        {
            request.Id= id;
            var result = await _mediator.Send(request);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllUserBooks()
        {
            var result = await _mediator.Send(new GetUserBooksQueryRequest ());
            return Ok(result);
        }
    }
}
