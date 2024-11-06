using BistroQ.Core.Dtos;
using BistroQ.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BistroQ.Core.Contracts.Services;
/// <summary>
/// Provides methods for securely storing and retrieving authentication tokens, including access and refresh tokens.
/// </summary>
public interface ITokenStorageService
{
    /// <summary>
    /// Stores the access and refresh tokens, along with additional information, from a login result.
    /// </summary>
    /// <param name="result">The login result containing tokens and other authentication details.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveTokensAsync(LoginResult result);

    /// <summary>
    /// Stores the access token.
    /// </summary>
    /// <param name="accessToken">The access token to store.</param>
    /// <returns>A task that represents the asynchronous save operation.</returns>
    Task SaveAccessToken(string accessToken);

    /// <summary>
    /// Retrieves the stored access token.
    /// </summary>
    /// <returns>A task that represents the asynchronous retrieval operation. The task result contains the stored access token as a string.</returns>
    Task<string> GetAccessToken();

    /// <summary>
    /// Retrieves the stored refresh token along with the associated user ID.
    /// </summary>
    /// <returns>A task that represents the asynchronous retrieval operation. 
    /// The task result contains a tuple with the refresh token and user ID.</returns>
    Task<(string refreshToken, string userId)> GetRefreshToken();

    /// <summary>
    /// Clears all stored tokens, including access and refresh tokens, to effectively log out the user.
    /// </summary>
    /// <returns>A task that represents the asynchronous clear operation.</returns>
    Task ClearTokensAsync();

    /// <summary>
    /// Get the current user's role.
    /// </summary>
    /// <returns>User's role</returns>
    Task<string> GetRoleAsync();

}
