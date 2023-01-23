namespace ApiClient.Policies;

public interface IRetryPolicyConfig
{
    int RetryCount { get; set; }
}