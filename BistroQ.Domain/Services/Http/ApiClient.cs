using BistroQ.Domain.Contracts.Services;

namespace BistroQ.Domain.Services.Http;

public class ApiClient : BaseApiClient, IApiClient
{
    public ApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
