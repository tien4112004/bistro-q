using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services.Auth;

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

    public async Task<string> GetTokenAsync()
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
            await LogoutAsync();

            throw;
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
}
