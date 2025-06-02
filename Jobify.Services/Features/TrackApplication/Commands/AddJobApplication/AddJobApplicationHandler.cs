using Jobify.Domain.Models;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Application.Features.TrackApplication.Commands.AddJobApplication
{
    public class AddJobApplicationHandler : IRequestHandler<AddJobApplicationCommand, ApiResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AddJobApplicationHandler> _logger;
        public AddJobApplicationHandler(IApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AddJobApplicationHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ApiResponse> Handle(AddJobApplicationCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "User not authenticated",
                        StatusCode = 401
                    };
                }

                var jobApplication = new JobApplication
                {
                    JobSeekerId = userId,
                    JobTitle = request.JobTitle,
                    Company = request.Company,
                    SkillsRequired = request.SkillsRequired,
                    CombinedMatchScore = request.CombinedMatchScore,
                    JobLink = request.JobLink,
                    Status = request.Status,// default is "Applied" should be set in the command
                    UpdateDate = request.UpdateDate

                };

                _context.JobApplications.Add(jobApplication);
                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Job application created successfully for user {UserId}, Job: {JobTitle} at {Company}",
                    userId, request.JobTitle, request.Company);

                return new ApiResponse
                {
                    Success = true,
                    Message = "Job application created successfully",
                    StatusCode = 201
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating job application for user");
                return new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while creating the job application",
                    StatusCode = 500
                };
            }

        }
    }
}
