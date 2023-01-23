using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using Polly.Retry;
using Shared.Common;

namespace ApiClient.Services;

public partial class SayHelloHttpService : ISayHelloHttpService
{
    private readonly HttpClient _httpClient;
    private static string _authorizationTokenStore = "";

    public SayHelloHttpService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<string> GetHelloAsync(CancellationToken cancellationToken)
    {
        //Create Auth Policy
        var policy = GetAuthorizationPolicy(cancellationToken);
        
        // var res = await Policy.WrapAsync(policy, policy).ExecuteAsync(() =>
        // {
        //  
        // });
        //  
        
        
        var response = await policy.ExecuteAsync(context =>
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _authorizationTokenStore);

            return _httpClient.GetAsync("api/hello");
        }, cancellationToken);
        
        var apiResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        return apiResponse;
    }
}

public partial class SayHelloHttpService
{
    private AsyncRetryPolicy<HttpResponseMessage> GetAuthorizationPolicy(CancellationToken cancellationToken)
    {
        var policy = Policy.HandleResult<HttpResponseMessage>(msg => msg.StatusCode == HttpStatusCode.Unauthorized)
            .RetryAsync(1,
                async (results, retryCount, context) => { await GetOrCreateRefreshJwtToken(cancellationToken); });
        return policy;
    }

    private async Task GetOrCreateRefreshJwtToken(CancellationToken cancellationToken)
    {
        var content = new StringContent(JsonConvert.SerializeObject(CommonConstants.SecurityKey), Encoding.UTF8,
            "application/json");
        var response = await _httpClient.PostAsync("api/token", content, cancellationToken);

        var apiResponse = await response.Content.ReadAsStringAsync(cancellationToken);

        var token = JObject.Parse(apiResponse).GetValue("token")?.ToString();

        _authorizationTokenStore = token;
    }
}