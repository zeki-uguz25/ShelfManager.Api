using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Users.Commands;
using ShelfManager.Application.Handlers.Users.Queries;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await _mediator.Send(new GetAllUsersQueryRequest());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQueryRequest { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}/ban")]
        public async Task<IActionResult> BanUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new BanUserCommandRequest { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}/unban")]
        public async Task<IActionResult> UnbanUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new UnbanUserCommandRequest { Id = id });
            return Ok(result);
        }
    }
}
