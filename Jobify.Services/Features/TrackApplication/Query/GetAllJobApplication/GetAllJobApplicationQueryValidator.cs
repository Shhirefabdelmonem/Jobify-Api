using FluentValidation;

namespace Jobify.Application.Features.TrackApplication.Query.GetAllJobApplication
{
    public class GetAllJobApplicationQueryValidator : AbstractValidator<GetAllJobApplicationQuery>
    {
        public GetAllJobApplicationQueryValidator()
        {
            // No validation rules needed for GetAllJobApplicationQuery
            // This validator exists to prevent validation errors from other validators being applied incorrectly
        }
    }
} 