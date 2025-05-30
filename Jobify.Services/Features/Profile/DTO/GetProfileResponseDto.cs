using Jobify.Services.Features.Profile.Commands.UpdateProfile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Features.Profile.DTO
{
    public class GetProfileResponseDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string Portfolio { get; set; }
        public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
        public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();
        public List<SkillDto> Skills { get; set; } = new List<SkillDto>();
    }
}
