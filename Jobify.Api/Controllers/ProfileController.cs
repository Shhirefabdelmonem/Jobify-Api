using Jobify.Services.Features.Profile.Commands.UpdateProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator)
        {
            _mediator= mediator;
        }

        [HttpPost("Update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileCommand command)
        {
            var result = await _mediator.Send(command);
            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode,result);
        }
    }
}
