using MediatR;
using Jobify.Services.Commons.DTOs.Resoponses;
using Jobify.Services.Commons.DTOs.requests;
using Jobify.Services.Commons.Interfaces;
using Microsoft.Extensions.Logging;
using Jobify.Services.Features.Profile.Commands.GetProfile;
using Jobify.Services.Features.Profile.DTO;

namespace Jobify.Services.Features.recommendation.Queries.GetRecommendations
{
    public class GetRecommendationsQueryHandler : IRequestHandler<GetRecommendationsQuery, ApiResponse>
    {
        private readonly IRecommendationService _recommendationService;
        private readonly ILogger<GetRecommendationsQueryHandler> _logger;
        private readonly IMediator _mediator;


        public GetRecommendationsQueryHandler(
            IRecommendationService recommendationService,
            ILogger<GetRecommendationsQueryHandler> logger,
            IMediator mediator)
        {
            _recommendationService = recommendationService;
            _logger = logger;
            _mediator = mediator;
        }

        public async Task<ApiResponse> Handle(GetRecommendationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var res = await _mediator.Send(new GetProfileQuery(), cancellationToken);
                var profile = res.Data as GetProfileResponseDto;

                if (profile == null)
                {
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Unable to retrieve user profile",
                        StatusCode = 404
                    };
                }

                var recommendationRequest = new RecommendationRequestDto
                {
                    Name = !string.IsNullOrEmpty(profile.FirstName) ? profile.FirstName : "User",
                    Degree = profile.Educations?.Any() == true
                        ? String.Join(", ", profile.Educations.Select(e => e.DegreeType).Where(d => !string.IsNullOrEmpty(d)))
                        : "Bachelor's",
                    Major = profile.Educations?.Any() == true
                        ? String.Join(", ", profile.Educations.Select(e => e.Major).Where(m => !string.IsNullOrEmpty(m)))
                        : "Computer Science",
                    Gpa = profile.Educations?.FirstOrDefault()?.Gpa ?? 0,
                    Experience = profile.Experiences?.Count ?? 0,
                    Skills = profile.Skills?.Any() == true
                        ? String.Join(", ", profile.Skills.Select(s => s.SkillName).Where(s => !string.IsNullOrEmpty(s)))
                        : "General skills",
                };

                var recommendationsResponse = await _recommendationService.GetRecommendationsAsync(recommendationRequest, cancellationToken);

                if (recommendationsResponse == null)
                {
                    _logger.LogWarning("No recommendations received from the AI service for user: {Name}", recommendationRequest.Name);
                    return new ApiResponse
                    {
                        Success = false,
                        Message = "Unable to get recommendations at this time. Please try again later.",
                        StatusCode = 503
                    };
                }

                return new ApiResponse
                {
                    Success = true,
                    Message = "Recommendations retrieved successfully",
                    Data = recommendationsResponse,
                    StatusCode = 200
                };
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "HTTP error occurred while getting recommendations");
                return new ApiResponse
                {
                    Success = false,
                    Message = "Unable to connect to the recommendation service. Please try again later.",
                    StatusCode = 503
                };
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogError(ex, "Timeout occurred while getting recommendations");
                return new ApiResponse
                {
                    Success = false,
                    Message = "Request timeout. Please try again.",
                    StatusCode = 408
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while getting recommendations");
                return new ApiResponse
                {
                    Success = false,
                    Message = "An unexpected error occurred. Please try again later.",
                    StatusCode = 500
                };
            }
        }
    }
}