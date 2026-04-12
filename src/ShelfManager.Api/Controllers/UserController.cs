using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Users.Commands;
using ShelfManager.Application.Handlers.Users.Queries;
using ShelfManager.Domain.Constants;

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
        [Authorize(Policy = Permissions.Users.GetUser)]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _mediator.Send(new GetAllUsersQueryRequest
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> GetUserById([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new GetUserByIdQueryRequest { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}/ban")]
        [Authorize(Policy = Permissions.Users.Ban)]
        public async Task<IActionResult> BanUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new BanUserCommandRequest { Id = id });
            return Ok(result);
        }

        [HttpPut("{id}/unban")]
        [Authorize(Policy = Permissions.Users.Ban)]
        public async Task<IActionResult> UnbanUser([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new UnbanUserCommandRequest { Id = id });
            return Ok(result);
        }
    }
}
