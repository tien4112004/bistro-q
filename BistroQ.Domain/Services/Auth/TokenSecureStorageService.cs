using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Models.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace BistroQ.Domain.Services.Auth;

/// <summary>
/// A secure storage service for authentication tokens, implementing the <see cref="ITokenStorageService"/> interface.
/// This service provides methods to securely store, retrieve, and clear access and refresh tokens,
/// using encryption to ensure sensitive data is protected.
/// </summary>
public class TokenSecureStorageService : ITokenStorageService
{
    private readonly SemaphoreSlim _semaphore;
    private readonly string _folderPath;
    private readonly string _fileName;
    private readonly IFileService _fileService;
    private readonly byte[] _entropy;

    public TokenSecureStorageService(IFileService fileService)
    {
        _fileService = fileService;
        _semaphore = new SemaphoreSlim(1, 1);
        _entropy = Encoding.UTF8.GetBytes("BistroQ");
        // File path will be: ./bin/x64/Debug/net7.0-windows10.0.19041.0\AppX\Data
        _folderPath = Path.Combine(AppContext.BaseDirectory, "Data");
        _fileName = "tokens.dat";
    }
    public async Task SaveTokensAsync(LoginResponseDto responseDto)
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
            Task.Run(async () => res["AccessToken"] = await EncryptData(responseDto.AccessToken)),
            Task.Run(async () => res["RefreshToken"] = await EncryptData(responseDto.RefreshToken)),
            Task.Run(async () => res["UserId"] = await EncryptData(responseDto.UserId)),
            Task.Run(async () => res["Role"] = await EncryptData(responseDto.Role))
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

            if (tokens == null || !tokens.ContainsKey("AccessToken"))
            {
                throw new TokenStorageException("No access token found");
            }

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

            if (tokens == null || !tokens.ContainsKey("AccessToken") || !tokens.ContainsKey("UserId"))
            {
                throw new TokenStorageException("No tokens found");
            }

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
            if (result == null)
            {
                throw new TokenStorageException("No tokens found");
            }

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

    public async Task<string> GetRoleAsync()
    {
        await _semaphore.WaitAsync();

        try
        {
            var result = await Task.Run(() =>
            {
                return _fileService.Read<Dictionary<string, string>>(_folderPath, _fileName);
            });
            if (result == null || !result.ContainsKey("Role"))
            {
                return "User";
            }

            return await DecryptData(result["Role"]);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    /// <summary>
    /// Encrypts the specified data string using Data Protection API (DPAPI) for secure storage.
    /// </summary>
    /// <param name="data">The plain text data to be encrypted.</param>
    /// <returns>A task that represents the asynchronous encryption operation. 
    /// The task result contains the encrypted data as a Base64-encoded string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null or empty.</exception>
    private Task<string> EncryptData(string data)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data));

        var jsonData = JsonSerializer.Serialize(data);
        var dataBytes = Encoding.UTF8.GetBytes(jsonData);

        // Encrypt using DPAPI
#pragma warning disable CA1416 // Validate platform compatibility
        var encryptedData = ProtectedData.Protect(
            dataBytes,
            _entropy,
            DataProtectionScope.CurrentUser
        );

        return Task.FromResult(Convert.ToBase64String(encryptedData));
    }

    /// <summary>
    /// Decrypts an encrypted Base64-encoded string back into plain text using Data Protection API (DPAPI).
    /// </summary>
    /// <param name="data">The encrypted data as a Base64-encoded string.</param>
    /// <returns>A task that represents the asynchronous decryption operation. 
    /// The task result contains the decrypted plain text string.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="data"/> is null or empty.</exception>
    private Task<string> DecryptData(string data)
    {
        if (string.IsNullOrEmpty(data))
            throw new ArgumentNullException(nameof(data));

        var byteData = Convert.FromBase64String(data);

        // Decrypt using DPAPI
#pragma warning disable CA1416 // Validate platform compatibility
        var decryptedData = ProtectedData.Unprotect(
            byteData,
            _entropy,
            DataProtectionScope.CurrentUser
        );

        var jsonData = Encoding.UTF8.GetString(decryptedData);
        var dataString = JsonSerializer.Deserialize<string>(jsonData);

        return Task.FromResult(dataString);
    }
}
