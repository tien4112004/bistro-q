using BistroQ.Core.Contracts.Services;
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
    private const int TOKEN_EXPIRE_TIME = 5;
    private const int REF_TOKEN_EXPIRE_TIME= 60;

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
            var result = new AuthResult
            {
                AccessToken = DateTime.Now.AddMinutes(TOKEN_EXPIRE_TIME).ToString(),
                RefreshToken = DateTime.Now.AddMinutes(REF_TOKEN_EXPIRE_TIME).ToString(),
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

    public Task<string> RefreshTokenAsync(string userId, string refreshToken)
    {
        throw new NotImplementedException();
    }
}
