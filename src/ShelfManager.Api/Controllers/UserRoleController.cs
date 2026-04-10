using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.UserRoles.Commands;
using ShelfManager.Application.Handlers.UserRoles.Queries;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserRoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserRole([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetUserRoleQueryRequest { UserId = userId });
            return Ok(result);
        }

        [HttpPut("{userId}/assign/{roleId}")]
        public async Task<IActionResult> AssignRole([FromRoute] Guid userId, [FromRoute] Guid roleId)
        {
            var result = await _mediator.Send(new AssignRoleCommandRequest { UserId = userId, RoleId = roleId });
            return Ok(result);
        }
    }
}
