using BistroQ.Core.Contracts.Services;

namespace BistroQ.Core.Services.Http;

public class ApiClient : BaseApiClient, IApiClient
{
    public ApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
