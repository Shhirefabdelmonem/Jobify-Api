using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jobify.Core.ValueObjects
{
    public static  class ApplicationStatus
    {
        public const string Applied = "Applied";
        public const string GetAssessment = "Get Assessment";
        public const string Interviewing = "Interviewing";
        public const string OfferReceived = "Offer Received";
        public const string Rejected = "Rejected";
        public const string Archived = "Archived";
        public static IEnumerable<string> GetAllStatuses()
        {
            return new List<string>
            {
                Applied,
                Interviewing,
                GetAssessment,
                OfferReceived,
                Rejected,
                Archived
            };
        }
    }
}
