using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services.Mock;

public class MockAuthService : IAuthService
{
    private readonly ITokenStorageService _tokenStorageService;
    private const int TOKEN_EXPIRE_TIME = 1;
    private const int REF_TOKEN_EXPIRE_TIME= 2;

    public MockAuthService(ITokenStorageService tokenStorageService)
    {
        _tokenStorageService = tokenStorageService;
    }

    public async Task<(bool Success, string Message)> LoginAsync(string username, string password)
    {
        await Task.Delay(1000);

        // Mock login
        if (username == "test" && password == "test")
        {
            var result = new LoginResult
            {
                AccessToken = "1," + DateTime.Now.AddMinutes(TOKEN_EXPIRE_TIME).ToString(),
                RefreshToken = "1," + DateTime.Now.AddMinutes(REF_TOKEN_EXPIRE_TIME).ToString(),
                UserId = "1",
                Role = "Admin"
            };

            await _tokenStorageService.SaveTokensAsync(result);
        }
        else
        {
            return (false, "Invalid username or password");
        }

        return (true, "Login successful");
    }


    public async Task RefreshTokenAsync()
    {
        var (refreshToken, userId) = await _tokenStorageService.GetRefreshToken();
        var tokens = refreshToken.Split(',');

        var userIdFromToken = tokens[0];
        var expireTime = DateTime.Parse(tokens[1]);

        if (userIdFromToken != userId)
        {
            throw new Exception("Unauthorized");
        }
        else if (expireTime < DateTime.Now)
        {
            throw new Exception("TokenExpired");
        }
        else
        {
            var accessToken = "1," + DateTime.Now.AddMinutes(TOKEN_EXPIRE_TIME).ToString();

            await _tokenStorageService.SaveAccessToken(accessToken);
        }
    }

    public async Task LogoutAsync()
    {
        await _tokenStorageService.ClearTokensAsync();
    }
}
