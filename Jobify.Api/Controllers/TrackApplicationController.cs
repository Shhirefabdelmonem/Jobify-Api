using Jobify.Application.Features.TrackApplication.Commands.AddJobApplication;
using Jobify.Application.Features.TrackApplication.Commands.UpdateJobApplication;
using Jobify.Application.Features.TrackApplication.Query.GetAllJobApplication;
using Jobify.Application.Features.TrackApplication.Query.GetJobApplicationByStatus;
using Jobify.Core.ValueObjects;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Jobify.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TrackApplicationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TrackApplicationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> AddJobApplication([FromBody] AddJobApplicationCommand command)
        {
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateJobApplication([FromRoute] int id, [FromBody] UpdateJobApplicationCommand command)
        {
            // Ensure the ID from route matches the command
            command.JobApplicationId = id;
            var result = await _mediator.Send(command);
            return StatusCode(result.StatusCode, result);
        }


        [HttpGet("all-applications")]
        public async Task<IActionResult> GetAllJobApplications() // Fixed typo in method name
        {
            var result = await _mediator.Send(new GetAllJobApplicationQuery());
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("by-status")]
        public async Task<IActionResult> GetJobApplicationsByStatus([FromQuery] string? status)
        {
            var query = new GetJobApplicationByStatusQuery { Status = status };
            var result = await _mediator.Send(query);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet("statuses")]
        public IActionResult GetAvailableStatuses()
        {
            var statuses = ApplicationStatus.GetAllStatuses();
            return Ok(new
            {
                Success = true,
                Message = "Available statuses retrieved successfully",
                Data = statuses,
                StatusCode = 200
            });
        }
    }

}
