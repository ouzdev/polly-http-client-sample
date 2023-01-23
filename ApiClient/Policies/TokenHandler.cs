using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ApiClient.Policies;

public class TokenHandler : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var authRequest = request;
        
        if (!string.IsNullOrEmpty(TokenDto.AccessToken))
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenDto.AccessToken);
            var response = await base.SendAsync(request, cancellationToken);
            return response;
        }


        var encoded =
            Convert.ToBase64String(Encoding.UTF8.GetBytes("turkey_Client:e_FjxpFRqLfMF1L-dK6D9zPS"));
        authRequest.RequestUri =
            new Uri("https://mtucn1uat.hospitality-api.us-ashburn-1.ocs.oc-test.com/oauth/v1/tokens");
        authRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", encoded);
        var parameters = new Dictionary<string, string>
        {
            { "username", "OHIPCN_PROTEL" },
            { "password", "PE/of]fPM3bamK}/9K8B1%$+" },
            { "grant_type", "password" }
        };
        authRequest.Method = HttpMethod.Post;
        authRequest.Content = new FormUrlEncodedContent(parameters);


        var responseAuth = await base.SendAsync(authRequest, cancellationToken);


        var content = await responseAuth.Content.ReadAsStringAsync(cancellationToken);

        var token = JsonSerializer.Deserialize<Token>(content);
        TokenDto.AccessToken = token.access_token;
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", TokenDto.AccessToken);
        var response2 = await base.SendAsync(request, cancellationToken);

        return response2;
    }
}
public class Token
{
    public int expires_in { get; set; }
    public string token_type { get; set; }
    public string oracle_tk_context { get; set; }
    public string refresh_token { get; set; }
    public string oracle_grant_type { get; set; }
    public string access_token { get; set; }
}