using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jobify.Services.Commons.DTOs.requests;
using Jobify.Services.Commons.DTOs.Resoponses;

namespace Jobify.Services.Commons.Interfaces
{
    public interface IRecommendationService
    {
        Task<RecommendationResponseDto?> GetRecommendationsAsync(RecommendationRequestDto request, CancellationToken cancellationToken = default);
    }
}
