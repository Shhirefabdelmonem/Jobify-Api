using Jobify.Core.Models;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using System;
using System.Collections.Generic;

namespace Jobify.Services.Features.Profile.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<ApiResponse>
    {
        // Personal Information
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string Portfolio { get; set; }

        // Education Information
        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();

        // Experience Information
        public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();

        // Skills
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
    }
}
