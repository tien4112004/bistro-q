using BistroQ.Core.Contracts.Services;

namespace BistroQ.Core.Services.Http;

public class PublicApiClient : BaseApiClient, IPublicApiClient
{
    public PublicApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
