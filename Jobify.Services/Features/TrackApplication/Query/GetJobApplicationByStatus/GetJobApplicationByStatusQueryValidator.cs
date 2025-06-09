using FluentValidation;

namespace Jobify.Application.Features.TrackApplication.Query.GetJobApplicationByStatus
{
    public class GetJobApplicationByStatusQueryValidator : AbstractValidator<GetJobApplicationByStatusQuery>
    {
        public GetJobApplicationByStatusQueryValidator()
        {
            // Status is optional, so no validation rules are needed
            // This validator exists to prevent validation errors from other validators being applied incorrectly
        }
    }
}