namespace ApiClient.Policies;

public static class HttpClientBuilderExtensions
{
    public static IHttpClientBuilder AddPolicyHandlers(this IHttpClientBuilder httpClientBuilder,
        string policySectionName, ILoggerFactory loggerFactory, IConfiguration configuration)
    {
        var retryLogger = loggerFactory.CreateLogger("PollyHttpRetryPoliciesLogger");
        var circuitBreakerLogger = loggerFactory.CreateLogger("PollyHttpCircuitBreakerPoliciesLogger");

        var policyConfig = new PolicyConfig();
        configuration.Bind(policySectionName, policyConfig);

        var circuitBreakerPolicyConfig = (ICircuitBreakerPolicyConfig)policyConfig;
        var retryPolicyConfig = (IRetryPolicyConfig)policyConfig;
        //var authRetryPolicyConfig = (IAuthRetryPolicyConfig)policyConfig;

        return httpClientBuilder.AddRetryPolicyHandler(retryLogger, retryPolicyConfig)
            // .AddAuthRetryPolicyHandler(retryLogger, authRetryPolicyConfig)
            .AddCircuitBreakerHandler(circuitBreakerLogger, circuitBreakerPolicyConfig);
    }

    public static IHttpClientBuilder AddRetryPolicyHandler(this IHttpClientBuilder httpClientBuilder, ILogger logger,
        IRetryPolicyConfig retryPolicyConfig)
    {
        return httpClientBuilder.AddPolicyHandler(HttpRetryPolicies.GetHttpRetryPolicy(logger, retryPolicyConfig));
    }

    // public static IHttpClientBuilder AddAuthRetryPolicyHandler(this IHttpClientBuilder httpClientBuilder,
    //     ILogger logger, IAuthRetryPolicyConfig authRetryPolicyConfig)
    // {
    //     return httpClientBuilder.AddPolicyHandler(
    //         HttpAuthRetryPolicies.GetAuthorizationPolicy(logger, authRetryPolicyConfig));
    // }

    public static IHttpClientBuilder AddCircuitBreakerHandler(this IHttpClientBuilder httpClientBuilder, ILogger logger,
        ICircuitBreakerPolicyConfig circuitBreakerPolicyConfig)
    {
        return httpClientBuilder.AddPolicyHandler(
            HttpCircuitBreakerPolicies.GetHttpCircuitBreakerPolicy(logger, circuitBreakerPolicyConfig));
    }
}