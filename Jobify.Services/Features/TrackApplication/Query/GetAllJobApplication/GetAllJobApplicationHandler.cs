using Jobify.Application.Features.TrackApplication.Query.GetJobApplicationByStatus;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Application.Features.TrackApplication.Query.GetAllJobApplication
{
    public class GetAllJobApplicationHandler : IRequestHandler<GetAllJobApplicationQuery, ApiResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetAllJobApplicationHandler> _logger;
        public GetAllJobApplicationHandler(IApplicationDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<GetAllJobApplicationHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ApiResponse> Handle(GetAllJobApplicationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Starting GetAllJobApplicationQuery handler");

                // Get the current user ID
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized access attempt to get job applications - no user ID found");
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "User not authenticated",
                        StatusCode = 401
                    };
                }

                _logger.LogInformation("Retrieved user ID: {UserId}", userId);

                // Build the query
                var query = _context.JobApplications
                    .Where(ja => ja.JobSeekerId == userId);

                _logger.LogInformation("Built query for job applications");

                var jobApplications = await query
                    .OrderByDescending(ja => ja.UpdateDate)
                    .Select(ja => new JobApplicationDto
                    {
                        JobApplicationId = ja.JobApplicationId,
                        JobTitle = ja.JobTitle,
                        Company = ja.Company,
                        SkillsRequired = ja.SkillsRequired,
                        CombinedMatchScore = ja.CombinedMatchScore,
                        JobLink = ja.JobLink,
                        Status = ja.Status,
                        UpdateDate = ja.UpdateDate,
                        JobSeekerId = ja.JobSeekerId
                    })
                    .ToListAsync(cancellationToken);

                _logger.LogInformation("Retrieved {Count} job applications from database", jobApplications.Count);

                var totalCount = jobApplications.Count;

                var result = new JobApplicationListDto
                {
                    JobApplications = jobApplications,
                    TotalCount = totalCount
                };

                _logger.LogInformation("Successfully retrieved {Count} job applications for user {UserId}",
                    jobApplications.Count, userId);

                return new ApiResponse
                {
                    Success = true,
                    Message = $"Job applications retrieved successfully",
                    Data = result,
                    StatusCode = 200
                };

            }
            catch (Exception ex)
            {
                _logger.LogInformation("error happened ");
                _logger.LogError(ex, "Error occurred while retrieving all job applications. Error: {ErrorMessage}", ex.Message);
                return new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while retrieving job applications",
                    StatusCode = 500
                };
            }
        }
    }
}
