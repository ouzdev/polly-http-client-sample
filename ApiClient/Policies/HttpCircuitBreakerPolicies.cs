using Polly;
using Polly.CircuitBreaker;

namespace ApiClient.Policies;

public static class HttpCircuitBreakerPolicies
{
    public static AsyncCircuitBreakerPolicy<HttpResponseMessage> GetHttpCircuitBreakerPolicy(ILogger logger, ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
    {
        return HttpPolicyBuilders.GetBaseBuilder()
            .CircuitBreakerAsync(circuitBreakerPolicyConfig.RetryCount + 1,
                TimeSpan.FromSeconds(circuitBreakerPolicyConfig.BreakDuration),
                (result, breakDuration) =>
                {
                    OnHttpBreak(result, breakDuration, circuitBreakerPolicyConfig.RetryCount, logger);
                },
                () =>
                {
                    OnHttpReset(logger);
                });
    }

    private static void OnHttpBreak(DelegateResult<HttpResponseMessage> result, TimeSpan breakDuration, int retryCount, ILogger logger)
    {
        if (result == null) 
            throw new ArgumentNullException(nameof(result));
        
        logger.LogWarning("Service shutdown during {breakDuration} after {DefaultRetryCount} failed retries.", breakDuration, retryCount);
        throw new BrokenCircuitException("Service inoperative. Please try again later");
    }

    private static void OnHttpReset(ILogger logger)
    {
        logger.LogInformation("Service restarted.");
    }
}