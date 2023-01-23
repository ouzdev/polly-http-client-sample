using System.Net;
using Polly;
using Polly.Retry;

namespace ApiClient.Policies;

public static class HttpAuthRetryPolicies
{
    public static AsyncRetryPolicy<HttpResponseMessage> GetAuthorizationPolicy(ILogger logger, IAuthRetryPolicyConfig authRetryPolicyConfig)
    {
        return HttpPolicyBuilders.GetBaseBuilder().OrResult(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
            .RetryAsync(authRetryPolicyConfig.RetryCount,
                async (result, retryCount, context) =>
                {
                    GetOrCreateRefreshJwtToken(result, retryCount, context, logger);
                });
    }

    private static void GetOrCreateRefreshJwtToken(DelegateResult<HttpResponseMessage> result, int retryCount,
        Context context, ILogger logger)
    {
        if (result.Result != null)
        {
            
            logger.LogWarning(
                "Request failed with {StatusCode}. Waiting {timeSpan} before next retry. Retry attempt {retryCount}",
                result.Result.StatusCode, retryCount);
        }
        else
        {
            logger.LogWarning(
                "Request failed because network failure. Waiting {timeSpan} before next retry. Retry attempt {retryCount}",
                retryCount);
        }
    }
}