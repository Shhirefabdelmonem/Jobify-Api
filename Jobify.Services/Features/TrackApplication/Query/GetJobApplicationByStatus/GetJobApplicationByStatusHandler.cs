using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Application.Features.TrackApplication.Query.GetJobApplicationByStatus
{
    public class GetJobApplicationByStatusHandler : IRequestHandler<GetJobApplicationByStatusQuery, ApiResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<GetJobApplicationByStatusHandler> _logger;

        public GetJobApplicationByStatusHandler(
            IApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<GetJobApplicationByStatusHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<ApiResponse> Handle(GetJobApplicationByStatusQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the current user ID
                var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("Unauthorized access attempt to get job applications by status");
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "User not authenticated",
                        StatusCode = 401
                    };
                }

                // Build the query
                var query = _context.JobApplications
                    .Where(ja => ja.JobSeekerId == userId);

                // Apply status filter if provided
                if (!string.IsNullOrEmpty(request.Status))
                {
                    query = query.Where(ja => ja.Status == request.Status);
                }



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

                var totalCount = jobApplications.Count;


                var result = new JobApplicationListDto
                {
                    JobApplications = jobApplications,
                    TotalCount = totalCount
                };

                _logger.LogInformation("Successfully retrieved {Count} job applications with status '{Status}' for user {UserId}",
                    jobApplications.Count, request.Status, userId);

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
                _logger.LogError(ex, "Error occurred while retrieving job applications by status '{Status}'", request.Status);
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
