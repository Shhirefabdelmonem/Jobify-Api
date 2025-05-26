using Jobify.Core.Models;
using Jobify.Services.Commons.Data;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Jobify.Services.Features.Profile.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, ApiResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UpdateProfileCommandHandler(
            IApplicationDbContext context, 
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<ApiResponse> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            // Get the current user ID directly from HttpContext
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
            var jobSeeker = await _context.JobSeekers
                .Include(js => js.Educations)
                .Include(js => js.Experiences)
                .Include(js => js.Skills)
                .FirstOrDefaultAsync(js => js.Id == userId, cancellationToken);

            if (jobSeeker == null)
            {
                // Create a new JobSeeker if not exists
                jobSeeker = new JobSeeker
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Email = request.Email
                };
                _context.JobSeekers.Add(jobSeeker);
            }

            // Update personal information
            jobSeeker.FirstName = request.FirstName;
            jobSeeker.LastName = request.LastName;
            jobSeeker.Email = request.Email;
            jobSeeker.Phone = request.Phone;
            jobSeeker.LinkedIn = request.LinkedIn;
            jobSeeker.GitHub = request.GitHub;
            jobSeeker.Portfolio = request.Portfolio;

            // Update educations
            await UpdateEducations(jobSeeker, request.Educations, cancellationToken);

            // Update experiences
            await UpdateExperiences(jobSeeker, request.Experiences, cancellationToken);

            // Update skills
            await UpdateSkills(jobSeeker, request.Skills, cancellationToken);

            try
            {
                // Save changes
                await _context.SaveChangesAsync(cancellationToken);

                return new ApiResponse
                {
                    Success = true,
                    Message = "Profile updated successfully",
                    StatusCode = 200
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    Success = false,
                    Message = $"Failed to update profile: {ex.Message}",
                    StatusCode = 500
                };
            }
        }

        private async Task UpdateEducations(JobSeeker jobSeeker, List<EducationDto> educationDtos, CancellationToken cancellationToken)
        {
            // Get existing education IDs
            var existingEducationIds = jobSeeker.Educations.Select(e => e.Id).ToList();
            var requestEducationIds = educationDtos.Where(e => e.Id.HasValue).Select(e => e.Id.Value).ToList();

            // Find educations to delete (existing but not in request)
            var educationsToDelete = jobSeeker.Educations.Where(e => !requestEducationIds.Contains(e.Id)).ToList();
            foreach (var education in educationsToDelete)
            {
                _context.Educations.Remove(education);
            }

            // Update or add educations
            foreach (var educationDto in educationDtos)
            {
                if (educationDto.Id.HasValue && existingEducationIds.Contains(educationDto.Id.Value))
                {
                    // Update existing education
                    var existingEducation = jobSeeker.Educations.First(e => e.Id == educationDto.Id.Value);
                    existingEducation.SchoolName = educationDto.SchoolName;
                    existingEducation.Major = educationDto.Major;
                    existingEducation.DegreeType = educationDto.DegreeType;
                    existingEducation.Gpa = educationDto.Gpa;
                    existingEducation.StartDate = educationDto.StartDate;
                    existingEducation.EndDate = educationDto.EndDate;
                }
                else
                {
                    // Add new education
                    var newEducation = new Education
                    {
                        UserId = jobSeeker.Id,
                        SchoolName = educationDto.SchoolName,
                        Major = educationDto.Major,
                        DegreeType = educationDto.DegreeType,
                        Gpa = educationDto.Gpa,
                        StartDate = educationDto.StartDate,
                        EndDate = educationDto.EndDate
                    };
                    jobSeeker.Educations.Add(newEducation);
                }
            }
        }

        private async Task UpdateExperiences(JobSeeker jobSeeker, List<ExperienceDto> experienceDtos, CancellationToken cancellationToken)
        {
            // Get existing experience IDs
            var existingExperienceIds = jobSeeker.Experiences.Select(e => e.Id).ToList();
            var requestExperienceIds = experienceDtos.Where(e => e.Id.HasValue).Select(e => e.Id.Value).ToList();

            // Find experiences to delete (existing but not in request)
            var experiencesToDelete = jobSeeker.Experiences.Where(e => !requestExperienceIds.Contains(e.Id)).ToList();
            foreach (var experience in experiencesToDelete)
            {
                _context.Experiences.Remove(experience);
            }

            // Update or add experiences
            foreach (var experienceDto in experienceDtos)
            {
                if (experienceDto.Id.HasValue && existingExperienceIds.Contains(experienceDto.Id.Value))
                {
                    // Update existing experience
                    var existingExperience = jobSeeker.Experiences.First(e => e.Id == experienceDto.Id.Value);
                    existingExperience.JobTitle = experienceDto.JobTitle;
                    existingExperience.Company = experienceDto.Company;
                    existingExperience.JobType = experienceDto.JobType;
                    existingExperience.Location = experienceDto.Location;
                    existingExperience.StartDate = experienceDto.StartDate;
                    existingExperience.EndDate = experienceDto.EndDate;
                    existingExperience.Summary = experienceDto.Summary;
                }
                else
                {
                    // Add new experience
                    var newExperience = new Experience
                    {
                        UserId = jobSeeker.Id,
                        JobTitle = experienceDto.JobTitle,
                        Company = experienceDto.Company,
                        JobType = experienceDto.JobType,
                        Location = experienceDto.Location,
                        StartDate = experienceDto.StartDate,
                        EndDate = experienceDto.EndDate,
                        Summary = experienceDto.Summary
                    };
                    jobSeeker.Experiences.Add(newExperience);
                }
            }
        }

        private async Task UpdateSkills(JobSeeker jobSeeker, List<SkillDto> skillDtos, CancellationToken cancellationToken)
        {
            // Get existing skill IDs
            var existingUserSkillIds = jobSeeker.Skills.Select(s => s.SkillId).ToList();
            var requestSkillIds = skillDtos.Where(s => s.Id.HasValue).Select(s => s.Id.Value).ToList();

            // Find skills to delete (existing but not in request)
            var skillsToDelete = jobSeeker.Skills.Where(s => !requestSkillIds.Contains(s.SkillId)).ToList();
            foreach (var skill in skillsToDelete)
            {
                _context.UserSkills.Remove(skill);
            }

            // Update or add skills
            foreach (var skillDto in skillDtos)
            {
                if (skillDto.Id.HasValue && existingUserSkillIds.Contains(skillDto.Id.Value))
                {
                    // Update existing skill
                    var existingUserSkill = jobSeeker.Skills.First(s => s.SkillId == skillDto.Id.Value);
                    
                    // Check if the skill exists in the Skills table
                    var skillEntity = await _context.Skills.FirstOrDefaultAsync(s => s.Name == skillDto.SkillName, cancellationToken);
                    if (skillEntity == null)
                    {
                        // Create new skill
                        skillEntity = new Skill { Name = skillDto.SkillName };
                        _context.Skills.Add(skillEntity);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    
                    existingUserSkill.SkillId = skillEntity.Id;
                }
                else
                {
                    // Check if the skill exists in the Skills table
                    var skillEntity = await _context.Skills.FirstOrDefaultAsync(s => s.Name == skillDto.SkillName, cancellationToken);
                    if (skillEntity == null)
                    {
                        // Create new skill
                        skillEntity = new Skill { Name = skillDto.SkillName };
                        _context.Skills.Add(skillEntity);
                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    // Add new user skill
                    var newUserSkill = new UserSkill
                    {
                        UserId = jobSeeker.Id,
                        SkillId = skillEntity.Id
                    };
                    jobSeeker.Skills.Add(newUserSkill);
                }
            }
        }
    }
}
