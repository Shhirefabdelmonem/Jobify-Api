using Jobify.Core.Models;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Features.Profile.Commands.UpdateProfile;
using Jobify.Services.Features.Profile.DTO;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Jobify.Services.Features.Profile.Commands.GetProfile
{
    public class GetProfileQueryHandler : IRequestHandler<GetProfileQuery, ApiResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GetProfileQueryHandler(
            IApplicationDbContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            try
            {
                // Get the current user ID from HttpContext
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

                // Get the user
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "User not found",
                        StatusCode = 404
                    };
                }

                // Check if user is a JobSeeker
                if (user is not JobSeeker)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "User is not authorized to access this resource",
                        StatusCode = 403
                    };
                }

                // Get JobSeeker with related data
                var jobSeeker = await _context.JobSeekers
                    .Include(js => js.Educations)
                    .Include(js => js.Experiences)
                    .Include(js => js.Skills)
                        .ThenInclude(us => us.Skill)
                    .FirstOrDefaultAsync(js => js.Id == userId, cancellationToken);

                if (jobSeeker == null)
                {
                    // Return empty profile if JobSeeker doesn't exist yet
                    return new ApiResponse
                    {
                        Success = true,
                        Message = "Profile retrieved successfully",
                        StatusCode = 200,
                        Data = new GetProfileResponseDto
                        {
                            FirstName = user.UserName, // Fallback to username if no profile exists
                            LastName = "",
                            Email = user.Email,
                            Phone = "",
                            LinkedIn = "",
                            GitHub = "",
                            Portfolio = "",
                            Educations = new List<EducationDto>(),
                            Experiences = new List<ExperienceDto>(),
                            Skills = new List<SkillDto>()
                        }
                    };
                }

                // Map JobSeeker to response DTO
                var profileResponse = new GetProfileResponseDto
                {
                    FirstName = jobSeeker.FirstName ?? "",
                    LastName = jobSeeker.LastName ?? "",
                    Email = jobSeeker.Email ?? user.Email,
                    Phone = jobSeeker.Phone ?? "",
                    LinkedIn = jobSeeker.LinkedIn ?? "",
                    GitHub = jobSeeker.GitHub ?? "",
                    Portfolio = jobSeeker.Portfolio ?? "",
                    Educations = jobSeeker.Educations.Select(e => new EducationDto
                    {
                        Id = e.Id,
                        SchoolName = e.SchoolName,
                        Major = e.Major,
                        DegreeType = e.DegreeType,
                        Gpa = e.Gpa,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate
                    }).ToList(),
                    Experiences = jobSeeker.Experiences.Select(e => new ExperienceDto
                    {
                        Id = e.Id,
                        JobTitle = e.JobTitle,
                        Company = e.Company,
                        JobType = e.JobType,
                        Location = e.Location,
                        StartDate = e.StartDate,
                        EndDate = e.EndDate,
                        Summary = e.Summary
                    }).ToList(),
                    Skills = jobSeeker.Skills.Select(s => new SkillDto
                    {
                        Id = s.SkillId,
                        SkillName = s.Skill.Name
                    }).ToList()
                };

                return new ApiResponse
                {
                    Success = true,
                    Message = "Profile retrieved successfully",
                    StatusCode = 200,
                    Data = profileResponse
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Failed to retrieve profile: {ex.Message}",
                    StatusCode = 500
                };
            }
        }
    }
}