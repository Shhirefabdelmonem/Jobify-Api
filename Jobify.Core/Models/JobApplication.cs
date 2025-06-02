using Jobify.Core.Models;
using Jobify.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Jobify.Domain.Models
{
    public class JobApplication
    {
        public int JobApplicationId { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string SkillsRequired { get; set; }
        public string CombinedMatchScore { get; set; }
        public string JobLink { get; set; }

        public string Status { get; set; } = ApplicationStatus.Applied;

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;

        public string JobSeekerId { get; set; }
        [ForeignKey("JobSeekerId")]
        [JsonIgnore]
        public virtual JobSeeker JobSeeker { get; set; }


    }
}
