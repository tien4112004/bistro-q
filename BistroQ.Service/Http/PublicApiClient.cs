using BistroQ.Domain.Contracts.Services;

namespace BistroQ.Domain.Services.Http;

/// <summary>
/// Implements a public API client without authentication requirements.
/// Inherits from BaseApiClient to provide basic REST API functionality.
/// </summary>
public class PublicApiClient : BaseApiClient, IPublicApiClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PublicApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for making requests.</param>
    public PublicApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}