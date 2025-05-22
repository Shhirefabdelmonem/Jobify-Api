using Jobify.Services.Features.Auth.Commands.Login;
using Jobify.Services.Features.Auth.Commands.RefreshToken;
using Jobify.Services.Features.Auth.Commands.Register;
using Jobify.Services.Features.Auth.Commands.RevokeToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> RevokeToken([FromBody] RevokeTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
