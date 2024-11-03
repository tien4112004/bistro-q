using BistroQ.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace BistroQ.Core.Services.Http;


public class AuthenticationDelegatingHandler : DelegatingHandler
{
    private readonly IAuthService _authService;

    public AuthenticationDelegatingHandler(IAuthService authService)
    {
        _authService = authService;
    }

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

    private async Task AddAuthenticationHeader(HttpRequestMessage request)
    {
        var token = await _authService.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    // The reason for cloning the request is that HttpRequestMessage can only be sent once. After it's sent, it's disposed and can't be reused.
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
}
