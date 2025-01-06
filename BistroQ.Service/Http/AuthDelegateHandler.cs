using BistroQ.Domain.Contracts.Services;
using System.Net;
using System.Net.Http.Headers;

namespace BistroQ.Domain.Services.Http;

/// <summary>
/// Handles HTTP request authentication by automatically adding and refreshing authentication tokens.
/// Implements a delegating handler pattern to intercept and modify HTTP requests.
/// </summary>
/// <remarks>
/// This handler:
/// - Adds authentication headers to outgoing requests
/// - Handles 401 Unauthorized responses by attempting to refresh the token
/// - Automatically retries failed requests with the new token
/// - Triggers logout on authentication failures
/// </remarks>
public class AuthenticationDelegatingHandler : DelegatingHandler
{
    #region Private Fields
    /// <summary>
    /// Service responsible for managing authentication tokens and operations.
    /// </summary>
    private readonly IAuthService _authService;
    #endregion

    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="AuthenticationDelegatingHandler"/> class.
    /// </summary>
    /// <param name="authService">The authentication service used for token management.</param>
    public AuthenticationDelegatingHandler(IAuthService authService)
    {
        _authService = authService;
    }
    #endregion

    #region Protected Methods
    /// <summary>
    /// Processes HTTP requests by adding authentication and handling token refresh scenarios.
    /// </summary>
    /// <param name="request">The HTTP request message to send.</param>
    /// <param name="cancellationToken">Token for cancelling the operation.</param>
    /// <returns>
    /// A <see cref="Task{HttpResponseMessage}"/> representing the result of the HTTP request.
    /// </returns>
    /// <remarks>
    /// The method implements a token refresh strategy:
    /// 1. Adds the current authentication token
    /// 2. Sends the request
    /// 3. If unauthorized (401):
    ///    - Attempts to refresh the token
    ///    - Retries the request with the new token
    ///    - Logs out on failure
    /// </remarks>
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Add token to initial request
        await AddAuthenticationHeader(request);

        // Send the request
        var response = await base.SendAsync(request, cancellationToken);

        // If unauthorized, try refresh and retry once
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            try
            {
                // Just refresh - no need for complex concurrency
                var newToken = await _authService.RefreshTokenAsync();

                var newRequest = await CloneRequest(request);
                newRequest.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", newToken);

                return await base.SendAsync(newRequest, cancellationToken);
            }
            catch (Exception)
            {
                await _authService.LogoutAsync();
                throw;
            }
        }

        return response;
    }
    #endregion

    #region Private Methods
    /// <summary>
    /// Adds the authentication header to the HTTP request.
    /// </summary>
    /// <param name="request">The HTTP request to modify.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    private async Task AddAuthenticationHeader(HttpRequestMessage request)
    {
        var token = await _authService.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Creates a deep copy of an HTTP request message.
    /// </summary>
    /// <param name="request">The request to clone.</param>
    /// <returns>A new instance of <see cref="HttpRequestMessage"/> with the same content and headers.</returns>
    /// <remarks>
    /// HttpRequestMessage can only be sent once. After it's sent, it's disposed and can't be reused.
    /// This method creates a complete copy that can be sent as a new request.
    /// </remarks>
    private async Task<HttpRequestMessage> CloneRequest(HttpRequestMessage request)
    {
        var newRequest = new HttpRequestMessage(request.Method, request.RequestUri);

        foreach (var header in request.Headers)
        {
            newRequest.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        if (request.Content != null)
        {
            var contentBytes = await request.Content.ReadAsByteArrayAsync();
            newRequest.Content = new ByteArrayContent(contentBytes);

            foreach (var header in request.Content.Headers)
            {
                newRequest.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        return newRequest;
    }
    #endregion
}