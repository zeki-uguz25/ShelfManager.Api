using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Fines.Commands;
using ShelfManager.Application.Handlers.Fines.Queries;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FineController : ControllerBase
    {
        private readonly IMediator _mediator;

        public FineController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserFines()
        {
            var result = await _mediator.Send(new GetUserFinesQueryRequest ());
            return Ok(result);
        }

        [HttpPut("{id}/pay")]
        [Authorize]
        public async Task<IActionResult> PayFine([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new PayFineCommandRequest { Id = id });
            return Ok(result);
        }
    }
}
