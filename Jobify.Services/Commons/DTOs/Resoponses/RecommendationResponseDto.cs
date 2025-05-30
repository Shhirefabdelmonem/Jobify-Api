using System.Text.Json.Serialization;

namespace Jobify.Services.Commons.DTOs.Resoponses
{
    public class RecommendationResponseDto
    {
        [JsonPropertyName("recommended_jobs")]
        public List<RecommendedJobDto> RecommendedJobs { get; set; } = new();
    }
}