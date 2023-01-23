namespace ApiClient.Policies;

public class PolicyConfig : ICircuitBreakerPolicyConfig, IRetryPolicyConfig,IAuthRetryPolicyConfig
{
    public int RetryCount { get; set; }
    public int BreakDuration { get; set; }
}