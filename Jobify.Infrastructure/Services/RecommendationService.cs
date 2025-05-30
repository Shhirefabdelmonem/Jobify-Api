using Jobify.Services;
using Jobify.Services.Commons.DTOs.requests;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Commons.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;

namespace Jobify.Infrastructure.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly RecommendationSettings _settings;
        private readonly ILogger<RecommendationService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;

        public RecommendationService(
            HttpClient httpClient,
            RecommendationSettings settings,
            ILogger<RecommendationService> logger)
        {
            _httpClient = httpClient;
            _settings = settings;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }

        public async Task<RecommendationResponseDto?> GetRecommendationsAsync(
            RecommendationRequestDto request,
            CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogInformation("Sending recommendation request for user: {Name}", request.Name);

                var requestJson = JsonSerializer.Serialize(request, _jsonOptions);
                _logger.LogInformation("Request JSON: {RequestJson}", requestJson);

                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                // Use relative URL since BaseAddress is set on HttpClient
                var endpoint = _settings.RecommendationEndpoint.TrimStart('/');
                _logger.LogInformation("Sending request to endpoint: {Endpoint}", endpoint);

                using var response = await _httpClient.PostAsync(endpoint, content, cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync(cancellationToken);
                    _logger.LogError("FastAPI service returned error status: {StatusCode}, Reason: {ReasonPhrase}, Content: {ErrorContent}",
                        response.StatusCode, response.ReasonPhrase, errorContent);
                    return null;
                }

                var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                if (string.IsNullOrWhiteSpace(responseContent))
                {
                    _logger.LogWarning("Empty response received from FastAPI service");
                    return null;
                }

                var recommendationResponse = JsonSerializer.Deserialize<RecommendationResponseDto>(responseContent, _jsonOptions);

                _logger.LogInformation("Successfully received {Count} recommendations for user: {Name}",
                    recommendationResponse?.RecommendedJobs?.Count ?? 0, request.Name);

                return recommendationResponse;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP request failed while getting recommendations for user: {Name}", request.Name);
                throw;
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                _logger.LogError(ex, "Request timeout while getting recommendations for user: {Name}", request.Name);
                throw;
            }
            catch (JsonException ex)
            {
                _logger.LogError(ex, "Failed to deserialize response from FastAPI service for user: {Name}", request.Name);
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting recommendations for user: {Name}", request.Name);
                throw;
            }
        }
    }
}