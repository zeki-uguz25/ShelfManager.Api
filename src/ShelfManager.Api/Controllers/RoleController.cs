using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Roles.Commands;
using ShelfManager.Application.Handlers.Roles.Queries;
using ShelfManager.Domain.Constants;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Authorize(Policy = Permissions.Roles.Manage)]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await _mediator.Send(new GetAllRolesQueryRequest());
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = Permissions.Roles.Manage)]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = Permissions.Roles.Manage)]
        public async Task<IActionResult> DeleteRole([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new DeleteRoleCommandRequest { Id = id });
            return NoContent();
        }
    }
}
