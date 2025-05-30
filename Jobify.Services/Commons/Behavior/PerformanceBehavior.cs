using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Jobify.Services.Commons.Behavior
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;
        private const int SlowRequestThresholdMs = 5000; // 5 seconds

        public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;
            var stopwatch = Stopwatch.StartNew();

            try
            {
                var response = await next();
                stopwatch.Stop();

                var elapsedMs = stopwatch.ElapsedMilliseconds;

                if (elapsedMs > SlowRequestThresholdMs)
                {
                    _logger.LogWarning("Slow request detected: {RequestName} took {ElapsedMs}ms to complete",
                        requestName, elapsedMs);
                }
                else
                {
                    _logger.LogInformation("{RequestName} completed in {ElapsedMs}ms",
                        requestName, elapsedMs);
                }

                return response;
            }
            catch (Exception ex)
            {
                stopwatch.Stop();
                _logger.LogError(ex, "{RequestName} failed after {ElapsedMs}ms",
                    requestName, stopwatch.ElapsedMilliseconds);
                throw;
            }
        }
    }
}