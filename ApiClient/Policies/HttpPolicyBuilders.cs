using Polly;
using Polly.Extensions.Http;

namespace ApiClient.Policies;

public static class HttpPolicyBuilders
{
    public static PolicyBuilder<HttpResponseMessage> GetBaseBuilder()
    {
        return HttpPolicyExtensions.HandleTransientHttpError();
    }
}