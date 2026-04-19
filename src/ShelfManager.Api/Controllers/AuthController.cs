using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShelfManager.Api.Common;
using ShelfManager.Application.Handlers.Auth.Commands;

namespace ShelfManager.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(ApiResponse<LoginCommandResponse>.Ok(result));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommandRequest request)
        {
            var result = await _mediator.Send(request);
            return Ok(ApiResponse<RegisterCommandResponse>.Ok(result));
        }
    }
}
