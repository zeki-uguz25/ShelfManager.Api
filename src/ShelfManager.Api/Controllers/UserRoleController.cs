using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.UserRoles.Commands;
using ShelfManager.Application.Handlers.UserRoles.Queries;
using ShelfManager.Domain.Constants;

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
        [Authorize(Policy = Permissions.Roles.Manage)]
        public async Task<IActionResult> GetUserRole([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetUserRoleQueryRequest { UserId = userId });
            return Ok(result);
        }

        [HttpPut("{userId}/assign/{roleId}")]
        [Authorize(Policy = Permissions.Roles.Manage)]
        public async Task<IActionResult> AssignRole([FromRoute] Guid userId, [FromRoute] Guid roleId)
        {
            var result = await _mediator.Send(new AssignRoleCommandRequest { UserId = userId, RoleId = roleId });
            return Ok(result);
        }
    }
}
