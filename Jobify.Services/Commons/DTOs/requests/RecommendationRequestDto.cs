using System.Text.Json.Serialization;

namespace Jobify.Services.Commons.DTOs.requests
{
    public class RecommendationRequestDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("degree")]
        public string Degree { get; set; } = string.Empty;

        [JsonPropertyName("major")]
        public string Major { get; set; } = string.Empty;

        [JsonPropertyName("gpa")]
        public decimal Gpa { get; set; } = 0.0m;

        [JsonPropertyName("experience")]
        public int Experience { get; set; } = 0;

        [JsonPropertyName("skills")]
        public string Skills { get; set; } = string.Empty;
    }
}