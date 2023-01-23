namespace ApiClient.Services;

public interface ISayHelloHttpService
{
    Task<string> GetHelloAsync(CancellationToken cancellationToken);
}