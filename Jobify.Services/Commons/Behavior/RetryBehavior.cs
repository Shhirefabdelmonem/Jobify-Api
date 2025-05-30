using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Jobify.Services.Commons.Behavior
{
    public class RetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        private readonly ILogger<RetryBehavior<TRequest, TResponse>> _logger;
        private const int MaxRetryAttempts = 3;
        private const int DelayBetweenRetriesMs = 1000;

        public RetryBehavior(ILogger<RetryBehavior<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            var requestName = typeof(TRequest).Name;

            for (int attempt = 1; attempt <= MaxRetryAttempts; attempt++)
            {
                try
                {
                    return await next();
                }
                catch (HttpRequestException ex) when (attempt < MaxRetryAttempts)
                {
                    _logger.LogWarning(ex, "Attempt {Attempt} failed for {RequestName}. Retrying in {Delay}ms...",
                        attempt, requestName, DelayBetweenRetriesMs);

                    await Task.Delay(DelayBetweenRetriesMs * attempt, cancellationToken);
                }
                catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException && attempt < MaxRetryAttempts)
                {
                    _logger.LogWarning(ex, "Attempt {Attempt} timed out for {RequestName}. Retrying in {Delay}ms...",
                        attempt, requestName, DelayBetweenRetriesMs);

                    await Task.Delay(DelayBetweenRetriesMs * attempt, cancellationToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Non-retryable exception occurred on attempt {Attempt} for {RequestName}",
                        attempt, requestName);
                    throw;
                }
            }

            // This should never be reached due to the exception handling above
            throw new InvalidOperationException($"All {MaxRetryAttempts} retry attempts failed for {requestName}");
        }
    }
}