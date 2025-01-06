using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Models.Entities;

/// <summary>
/// Interface for managing account-related data operations through API endpoints.
/// Provides methods for CRUD operations on user accounts.
/// </summary>
public interface IAccountDataService
{
    /// <summary>
    /// Retrieves a paginated collection of accounts based on query parameters.
    /// </summary>
    /// <param name="query">Query parameters for filtering, sorting, and pagination</param>
    /// <returns>A collection response containing accounts and pagination information</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<ApiCollectionResponse<IEnumerable<Account>>> GetAccountsAsync(AccountCollectionQueryParams query);

    /// <summary>
    /// Retrieves a specific account by its user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user</param>
    /// <returns>The account details for the specified user</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<Account> GetAccountByIdAsync(string userId);

    /// <summary>
    /// Creates a new account with the provided information.
    /// </summary>
    /// <param name="request">The account creation details</param>
    /// <returns>The newly created account</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<Account> CreateAccountAsync(CreateAccountRequest request);

    /// <summary>
    /// Updates an existing account with the provided information.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to update</param>
    /// <param name="request">The account update details</param>
    /// <returns>The updated account</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<Account> UpdateAccountAsync(string userId, UpdateAccountRequest request);

    /// <summary>
    /// Deletes an account with the specified user ID.
    /// </summary>
    /// <param name="userId">The unique identifier of the user to delete</param>
    /// <returns>True if the deletion was successful</returns>
    /// <exception cref="Exception">Thrown when the API request fails</exception>
    Task<bool> DeleteAccountAsync(string userId);
}