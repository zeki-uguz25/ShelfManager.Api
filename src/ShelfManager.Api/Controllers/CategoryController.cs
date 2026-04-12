using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Categories.Commands;
using ShelfManager.Application.Handlers.Categories.Queries;
using ShelfManager.Domain.Constants;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllCategoryQueryRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var result= await _mediator.Send(new GetCategoryByIdQueryRequest { Id=id});
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Categories.Manage)]
        public async Task<IActionResult> Create([FromBody] CreateCategoryCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize(Policy = Permissions.Categories.Manage)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateCategoryCommandRequest request)
        {
            request.Id = id;
            var result= await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Categories.Manage)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new  DeleteCategoryCommandRequest { Id=id});
            return Ok("Kategori silindi.");
        }
    }
}
