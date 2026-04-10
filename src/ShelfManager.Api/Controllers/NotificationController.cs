using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Application.Handlers.Notifications.Commands;
using ShelfManager.Application.Handlers.Notifications.Queries;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserNotifications()
        {
            var result = await _mediator.Send(new GetUserNotificationsQueryRequest ());
            return Ok(result);
        }

        [HttpPut("{id}/read")]
        public async Task<IActionResult> MarkAsRead([FromRoute] Guid id)
        {
            var result = await _mediator.Send(new MarkAsReadCommandRequest { Id = id });
            return Ok(result);
        }
    }
}
