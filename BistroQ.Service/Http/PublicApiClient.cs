using BistroQ.Domain.Contracts.Services;

namespace BistroQ.Domain.Services.Http;

public class PublicApiClient : BaseApiClient, IPublicApiClient
{
    public PublicApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}
