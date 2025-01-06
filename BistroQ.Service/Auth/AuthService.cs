using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Models.Exceptions;

namespace BistroQ.Service.Auth;

/// <summary>
/// Implements authentication and authorization services, handling user login, token management,
/// and authentication state. This service acts as a bridge between the client application
/// and the authentication API endpoints.
/// </summary>
/// <remarks>
/// This service:
/// - Manages user authentication state
/// - Handles token refresh and storage
/// - Provides authentication status checks
/// - Coordinates with the token storage service for persistence
/// </remarks>
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
        var response = await _apiClient.PostAsync<LoginResponseDto>("api/auth/login", new { username, password });

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

        var response = await _apiClient.PostAsync<RefreshTokenResponse>("api/Auth/refresh", new { RefreshToken = refreshToken, UserId = userId });

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