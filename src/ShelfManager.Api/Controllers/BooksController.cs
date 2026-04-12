using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Books.Queries;
using ShelfManager.Domain.Constants;
using ShelfManager.Domain.Entities;
using static ShelfManager.Application.Handlers.Books.Commands.CreateBookCommand;
using static ShelfManager.Application.Handlers.Books.Commands.DeleteBookCommand;
using static ShelfManager.Application.Handlers.Books.Commands.UpdateBookCommand;
using static ShelfManager.Application.Handlers.Books.Queries.GetAllBooksQuery;
using static ShelfManager.Application.Handlers.Books.Queries.GetBookByIdQuery;

namespace ShelfManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator; //Bu sayede controller ile handler bağımlı olmaz

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllBooksQueryRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return Ok(result);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetBookByIdQueryRequest { Id = id }); //Burda requeste swagger da girdiğimiz değer veriliyor ve işlem controllerdan çıkıyor.
            return Ok(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetBooksByCategory([FromRoute] Guid categoryId)
        {
            var result = await _mediator.Send(new GetBooksByCategoryQueryRequest { CategoryId = categoryId });
            return Ok(result);
        }


        [HttpPost]
        [Authorize(Policy = Permissions.Books.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBookCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.Books.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBookCommandRequest request)
        {
            request.Id = id; //Request bize id olmadan gelir çünkü id yi route da aldık. O yüzden burda requeste id ekliyoruz.
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Books.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteBookCommandRequest { Id = id });
            return NoContent();
        }



    }
}