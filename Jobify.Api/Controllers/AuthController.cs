using Jobify.Services.DTOs.requests;
using Jobify.Services.IControllerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LogInDTO request)
        {
            var response = await _authService.LoginAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            var response = await _authService.RegisterAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO request)
        {
            var response = await _authService.RefreshTokenAsync(request);
            if (response.Success)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> LogOut([FromBody] string refreshToken)
        {
            await _authService.RevokeRefreshTokenAsync(refreshToken);
            return NoContent();
        }
    }
}
