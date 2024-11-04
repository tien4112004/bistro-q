using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Helpers;
using BistroQ.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Services.Auth;

public class TokenJsonStorageService : ITokenStorageService
{
    private readonly IFileService _fileService;
    private readonly string _folderPath;
    private readonly string _fileName;

    public TokenJsonStorageService(IFileService fileService)
    {
        // File path will be: ./bin/x64/Debug/net7.0-windows10.0.19041.0\AppX\Data
        _fileService = fileService;
        _folderPath = Path.Combine(AppContext.BaseDirectory, "Data");
        _fileName = "tokens.json";
    }

    public Task SaveTokensAsync(LoginResult authResult)
    {
        var tokens = new Dictionary<string, string>
        {
            { "AccessToken", authResult.AccessToken },
            { "RefreshToken", authResult.RefreshToken },
            { "UserId", authResult.UserId },
            { "Role", authResult.Role }
        };

        _fileService.Save(_folderPath, _fileName, tokens);

        return Task.CompletedTask;
    }

    public Task SaveAccessToken(string accessToken)
    {
        var result = _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);

        result["AccessToken"] = accessToken;

        _fileService.Save(_folderPath, _fileName, result);

        return Task.CompletedTask;
    }

    public Task<string> GetAccessToken()
    {
        var tokens = _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);

        return Task.FromResult(tokens?["AccessToken"]);
    }

    public Task<(string, string)> GetRefreshToken()
    {
        var tokens = _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);

        return Task.FromResult((tokens?["RefreshToken"], tokens?["UserId"]));
    }


    public Task ClearTokensAsync()
    {
        _fileService.Delete(_folderPath, _fileName);

        return Task.CompletedTask;
    }
}