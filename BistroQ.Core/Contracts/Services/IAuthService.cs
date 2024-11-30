namespace BistroQ.Core.Contracts.Services;

/// <summary>
/// Defines the methods required for authentication and authorization services,
/// including user login, token management, and logout functionality.
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Attempts to log in a user with the provided username and password.
    /// </summary>
    /// <param name="username">The username of the user attempting to log in.</param>
    /// <param name="password">The password of the user attempting to log in.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a tuple
    /// indicating whether the login was successful and a message with additional information.</returns>
    Task<(bool Success, string Message)> LoginAsync(string username, string password);

    /// <summary>
    /// Generates a new access token using a refresh token and user id to maintain an active session.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a new access token as a string.</returns>
    Task<string> RefreshTokenAsync();

    /// <summary>
    /// Logs out the current user, invalidating their session. Also clears any stored tokens.
    /// </summary>
    /// <returns>A task that represents the asynchronous logout operation.</returns>
    Task LogoutAsync();

    /// <summary>
    /// Retrieves the current access token for the authenticated user.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the current access token as a string.</returns>
    Task<string> GetTokenAsync();

    /// <summary>
    /// Checks if a user is currently authenticated.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains a boolean
    /// indicating whether the user is authenticated.</returns>
    Task<bool> IsAuthenticatedAsync();

    /// <summary>
    /// Gets the current user's role.
    /// </summary>
    /// <returns>User's role</returns>
    Task<string> GetRoleAsync();
}

