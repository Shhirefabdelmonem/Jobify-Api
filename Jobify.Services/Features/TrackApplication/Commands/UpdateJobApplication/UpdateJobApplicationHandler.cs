using Jobify.Application.Features.TrackApplication.Commands.AddJobApplication;
using Jobify.Core.Models;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Application.Features.TrackApplication.Commands.UpdateJobApplication
{
    public class UpdateJobApplicationHandler :IRequestHandler<UpdateJobApplicationCommand, ApiResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AddJobApplicationHandler> _logger;
        public UpdateJobApplicationHandler(IApplicationDbContext context,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AddJobApplicationHandler> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<ApiResponse> Handle(UpdateJobApplicationCommand request, CancellationToken cancellationToken)
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

                var jobApplication = await _context.JobApplications
                       .FirstOrDefaultAsync(ja => ja.JobApplicationId == request.JobApplicationId && ja.JobSeekerId == userId, cancellationToken);


                if (jobApplication == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Job application not found",
                        StatusCode = 404
                    };
                }

                jobApplication.JobTitle = request.JobTitle;
                jobApplication.Company = request.Company;
                jobApplication.SkillsRequired = request.SkillsRequired;
                jobApplication.CombinedMatchScore = request.CombinedMatchScore;
                jobApplication.JobLink = request.JobLink;
                jobApplication.Status = request.Status;
                jobApplication.UpdateDate = DateTime.UtcNow;

                await _context.SaveChangesAsync(cancellationToken);

                _logger.LogInformation("Job application {Id} updated successfully for user {UserId}",
                    request.JobApplicationId, userId);

                return new ApiResponse
                {
                    Success = true,
                    Message = "Job application updated successfully",
                    StatusCode = 200
                };
            }
            

             catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating job application {Id}", request.JobApplicationId);
                return new ApiResponse
                {
                    Success = false,
                    Message = "An error occurred while updating the job application",
                    StatusCode = 500
                };
            }

        }
    }
    
}
