using Jobify.Services.Features.Profile.Commands.GetProfile;
using Jobify.Services.Features.recommendation.Queries.GetRecommendations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jobify.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RecommendationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecommendationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("get-recommendations")]
        public async Task<IActionResult> GetRecommendations()
        {
            var result = await _mediator.Send(new GetRecommendationsQuery());

            if (result.Success)
            {
                return Ok(result);
            }
            return StatusCode(result.StatusCode, result);
        }

        /// <summary>
        /// Health check endpoint for recommendation service
        /// </summary>
        /// <returns>Service status</returns>
        [HttpGet("health")]
        [AllowAnonymous]
        public IActionResult Health()
        {
            return Ok(new { Status = "Healthy", Service = "RecommendationService", Timestamp = DateTime.UtcNow });
        }
    }
}