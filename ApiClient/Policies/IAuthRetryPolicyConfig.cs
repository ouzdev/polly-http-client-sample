namespace ApiClient.Policies;

public interface IAuthRetryPolicyConfig
{
    int RetryCount { get; set; }
}