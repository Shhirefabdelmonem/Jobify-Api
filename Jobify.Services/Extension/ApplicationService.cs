using FluentValidation;
using Jobify.Services.Commons.Behavior;
using Jobify.Services.Commons.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Jobify.Services.Extension
{
    public static class ApplicationService
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddValidatorsFromAssembly(typeof(ApplicationService).Assembly, includeInternalTypes: true);
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ApplicationService).Assembly);
                config.AddOpenBehavior(typeof(LoggingBehavior<,>));
                config.AddOpenBehavior(typeof(ValidationBehavior<,>));
                config.AddOpenBehavior(typeof(PerformanceBehavior<,>));
                config.AddOpenBehavior(typeof(RetryBehavior<,>));
            });

            return services;
        }
    }
}
