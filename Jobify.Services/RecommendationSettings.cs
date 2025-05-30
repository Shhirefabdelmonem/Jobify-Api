namespace Jobify.Services
{
    public class RecommendationSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string RecommendationEndpoint { get; set; } = "/recommend";
        public int TimeoutSeconds { get; set; } = 30;
        public int MaxRetryAttempts { get; set; } = 3;
        public int RetryDelaySeconds { get; set; } = 2;
    }
}