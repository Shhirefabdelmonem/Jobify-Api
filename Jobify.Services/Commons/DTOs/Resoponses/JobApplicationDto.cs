using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Services.Commons.DTOs.Resoponses
{
    public class JobApplicationDto
    {
        public int JobApplicationId { get; set; }
        public string JobTitle { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string SkillsRequired { get; set; } = string.Empty;
        public string CombinedMatchScore { get; set; } = string.Empty;
        public string JobLink { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime UpdateDate { get; set; }
        public string JobSeekerId { get; set; } = string.Empty;
    }

    public class JobApplicationListDto
    {
        public List<JobApplicationDto> JobApplications { get; set; } = new();
        public int TotalCount { get; set; }
    }
}