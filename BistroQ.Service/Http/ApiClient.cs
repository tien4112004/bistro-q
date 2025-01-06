using BistroQ.Domain.Contracts.Services;

namespace BistroQ.Domain.Services.Http;

/// <summary>
/// Implements a secure API client with authentication capabilities.
/// Inherits from BaseApiClient to provide basic REST API functionality.
/// </summary>
public class ApiClient : BaseApiClient, IApiClient
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiClient"/> class.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for making requests.</param>
    public ApiClient(HttpClient httpClient) : base(httpClient)
    {
    }
}