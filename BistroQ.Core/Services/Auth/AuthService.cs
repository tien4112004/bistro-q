using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services.Auth;

/// <summary>
/// A JSON-based storage service for authentication tokens, implementing the <see cref="ITokenStorageService"/> interface.
/// This service stores access and refresh tokens in a JSON format, providing methods to save, retrieve,
/// and clear tokens. This approach may be useful for scenarios where tokens need to be persisted
/// in a lightweight, human-readable format.
/// </summary>
public class AuthService : IAuthService
{
    private readonly IPublicApiClient _apiClient;
    private readonly ITokenStorageService _tokenStorageService;

    public AuthService(IPublicApiClient apiClient, ITokenStorageService tokenStorageService)
    {
        _apiClient = apiClient;
        _tokenStorageService = tokenStorageService;
    }

    public async Task<(bool Success, string Message)> LoginAsync(string username, string password)
    {
        var response = await _apiClient.PostAsync<LoginResult>("api/auth/login", new { username, password });

        if (response.Success)
        {
            await _tokenStorageService.SaveTokensAsync(response.Data);
            return (true, response.Message ?? "Login successful");
        }

        return (false, response.Message ?? "Login failed");
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        try
        {
            await RefreshTokenAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        try
        {
            var token = await _tokenStorageService.GetAccessToken();

            if (string.IsNullOrEmpty(token))
            {
                throw new UnauthorizedException("Unauthorized");
            }

            return token;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task LogoutAsync()
    {
        await _tokenStorageService.ClearTokensAsync();
    }

    public async Task<string> RefreshTokenAsync()
    {
        var (refreshToken, userId) = await _tokenStorageService.GetRefreshToken();

        var response = await _apiClient.PostAsync<RefreshResult>("api/Auth/refresh", new { RefreshToken = refreshToken, UserId = userId });

        if (response.Success)
        {
            await _tokenStorageService.SaveAccessToken(response.Data.AccessToken);
            return response.Data.AccessToken;
        }
        else if (response.Error.Contains("TokenExpired"))
        {
            throw new TokenExpiredException("Token expired");
        }
        else
        {
            throw new UnauthorizedException("Unauthorized");
        }
    }

    public async Task<string> GetRoleAsync()
    {
        return await _tokenStorageService.GetRoleAsync();
    }
}
