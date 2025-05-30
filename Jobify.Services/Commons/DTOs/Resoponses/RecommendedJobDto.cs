using System.Text.Json.Serialization;

namespace Jobify.Services.Commons.DTOs.Resoponses
{
    public class RecommendedJobDto
    {
        [JsonPropertyName("job_title")]
        public string JobTitle { get; set; } 

        [JsonPropertyName("company")]
        public string Company { get; set; } 

        [JsonPropertyName("skills_required")]
        public string SkillsRequired { get; set; } 

        [JsonPropertyName("combined_match_score")]
        public string CombinedMatchScore { get; set; } 

        [JsonPropertyName("semantic_score")]
        public string SemanticScore { get; set; } 

        [JsonPropertyName("keyword_score")]
        public string KeywordScore { get; set; } 

        [JsonPropertyName("job_link")]
        public string JobLink { get; set; } 
    }
}