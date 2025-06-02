using Jobify.Core.ValueObjects;
using Jobify.Services.Commons.DTOs.Resoponses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Application.Features.TrackApplication.Commands.AddJobApplication
{
    public class AddJobApplicationCommand:IRequest<ApiResponse>
    {
        public int JobApplicationId { get; set; }
        public string JobTitle { get; set; }
        public string Company { get; set; }
        public string SkillsRequired { get; set; }
        public string CombinedMatchScore { get; set; }
        public string JobLink { get; set; }

        public string Status { get; set; } = ApplicationStatus.Applied;

        public DateTime UpdateDate { get; set; } = DateTime.UtcNow;
    }
}
