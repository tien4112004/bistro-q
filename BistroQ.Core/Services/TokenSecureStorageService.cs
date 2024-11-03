using BistroQ.Core.Contracts.Services;
using BistroQ.Core.Dtos;
using BistroQ.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BistroQ.Core.Services;

public class TokenSecureStorageService : ITokenStorageService
{
    private readonly SemaphoreSlim _semaphore;
    private readonly string _folderPath;
    private readonly string _fileName;
    private readonly IFileService _fileService;
    private readonly byte[] _entrophy;

    public TokenSecureStorageService(IFileService fileService)
    {
        _fileService = fileService;
        _semaphore = new SemaphoreSlim(1, 1);
        _entrophy = Encoding.UTF8.GetBytes("BistroQ");
        // File path will be: ./bin/x64/Debug/net7.0-windows10.0.19041.0\AppX\Data
        _folderPath = Path.Combine(AppContext.BaseDirectory, "Data");
        _fileName = "tokens.dat";
    }
    public async Task SaveTokensAsync(LoginResult result)
    {
        await _semaphore.WaitAsync();

        var res = new Dictionary<string, string>()
        {
            { "AccessToken", null },
            { "RefreshToken", null },
            { "UserId", null },
            { "Role", null }
        };

        var tasks = new Task[]
        {
            Task.Run(async () => res["AccessToken"] = await EncryptData(result.AccessToken)),
            Task.Run(async () => res["RefreshToken"] = await EncryptData(result.RefreshToken)),
            Task.Run(async () => res["UserId"] = await EncryptData(result.UserId)),
            Task.Run(async () => res["Role"] = await EncryptData(result.Role))
        };

        try
        {
            await Task.WhenAll(tasks);

            await Task.Run(() =>
            {
                _fileService.Save(_folderPath, _fileName, res);
            });
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task ClearTokensAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            await Task.Run(() =>
            {
                _fileService.Delete(_folderPath, _fileName);
            });
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<string> GetAccessToken()
    {
        await _semaphore.WaitAsync();

        try
        {
            var tokens = await Task.Run(() =>
            {
                return _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);
            });

            return await DecryptData(tokens["AccessToken"]);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task<(string refreshToken, string userId)> GetRefreshToken()
    {
        await _semaphore.WaitAsync();

        try
        {
            var tokens = await Task.Run(() =>
            {
                return _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);
            });

            return (
                await DecryptData(tokens["RefreshToken"]),
                await DecryptData(tokens["UserId"])
            );
        }
        finally
        {
            _semaphore.Release();
        }
    }

    public async Task SaveAccessToken(string accessToken)
    {
        await _semaphore.WaitAsync();

        try
        {
            var result = await Task.Run(() =>
            {
                return _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);
            });

            result["AccessToken"] = await EncryptData(accessToken);

            await Task.Run(() =>
            {
                _fileService.Save(_folderPath, _fileName, result);
            });
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private Task<string> EncryptData(string data)
    {
        var jsonData = JsonSerializer.Serialize(data);
        byte[] dataBytes = Encoding.UTF8.GetBytes(jsonData);
        byte[] _entrophy = Encoding.UTF8.GetBytes("BistroQ");

        // Encrypt using DPAPI
#pragma warning disable CA1416 // Validate platform compatibility
        byte[] encryptedData = ProtectedData.Protect(
            dataBytes,
            _entrophy,
            DataProtectionScope.CurrentUser
        );

        return Task.FromResult(Convert.ToBase64String(encryptedData));
    }

    private Task<string> DecryptData(string data)
    {
        byte[] byteData = Convert.FromBase64String(data);

        // Decrypt using DPAPI
#pragma warning disable CA1416 // Validate platform compatibility
        byte[] decryptedData = ProtectedData.Unprotect(
            byteData,
            _entrophy,
            DataProtectionScope.CurrentUser
        );

        string jsonData = Encoding.UTF8.GetString(decryptedData);
        string dataString = JsonSerializer.Deserialize<string>(jsonData);

        return Task.FromResult(dataString);
    }
}
