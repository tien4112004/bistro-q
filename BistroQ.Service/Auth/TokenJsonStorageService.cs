using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;

namespace BistroQ.Service.Auth;

/// <summary>
/// A JSON-based storage service for authentication tokens, implementing the <see cref="ITokenStorageService"/> interface.
/// This service stores access and refresh tokens in a JSON format, providing methods to save, retrieve,
/// and clear tokens. This approach may be useful for scenarios where tokens need to be persisted
/// in a lightweight, human-readable format.
/// </summary>
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

    public Task SaveTokensAsync(LoginResponseDto authResponseDto)
    {
        var tokens = new Dictionary<string, string>
        {
            { "AccessToken", authResponseDto.AccessToken },
            { "RefreshToken", authResponseDto.RefreshToken },
            { "UserId", authResponseDto.UserId },
            { "Role", authResponseDto.Role }
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

    public Task<string> GetRoleAsync()
    {
        throw new NotImplementedException();
    }
}