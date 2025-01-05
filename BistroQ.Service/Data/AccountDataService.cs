using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

/// <summary>
/// Implementation of account data service that communicates with the API endpoints.
/// Uses AutoMapper for data mapping between DTOs and domain models.
/// </summary>
public class AccountDataService : IAccountDataService
{
    #region Private Fields
    /// <summary>
    /// Client for making HTTP requests to the API.
    /// </summary>
    private readonly IApiClient _apiClient;

    /// <summary>
    /// Mapper for converting between DTOs and domain models.
    /// </summary>
    private readonly IMapper _mapper;
    #endregion

    #region Constructor
    public AccountDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }
    #endregion

    #region Public Methods
    public async Task<ApiCollectionResponse<IEnumerable<Account>>> GetAccountsAsync(AccountCollectionQueryParams query = null)
    {
        var response = await _apiClient.GetCollectionAsync<IEnumerable<AccountResponse>>("/api/admin/account", query);
        if (response.Success)
        {
            var accounts = _mapper.Map<IEnumerable<Account>>(response.Data);
            return new ApiCollectionResponse<IEnumerable<Account>>
                (accounts, response.TotalItems, response.CurrentPage, response.TotalPages);
        }

        throw new Exception("Failed to get accounts");
    }

    public async Task<Account> GetAccountByIdAsync(string userId)
    {
        var response = await _apiClient.GetAsync<AccountResponse>($"api/admin/account/{userId}", null);
        if (response.Success)
        {
            return _mapper.Map<Account>(response.Data);
        }

        throw new Exception("Failed to get account");
    }

    public async Task<Account> CreateAccountAsync(CreateAccountRequest request)
    {
        var response = await _apiClient.PostAsync<AccountResponse>("api/admin/account", request);
        if (response.Success)
        {
            return _mapper.Map<Account>(response.Data);
        }

        throw new Exception("Failed to create account");
    }

    public async Task<Account> UpdateAccountAsync(string userId, UpdateAccountRequest request)
    {
        var response = await _apiClient.PatchAsync<AccountResponse>($"api/admin/account/{userId}", request);
        if (response.Success)
        {
            return _mapper.Map<Account>(response.Data);
        }

        throw new Exception("Failed to update account");
    }

    public async Task<bool> DeleteAccountAsync(string userId)
    {
        var response = await _apiClient.DeleteAsync<object>($"api/admin/account/{userId}", null);
        if (!response.Success)
        {
            throw new Exception(response.Message);
        }

        return true;
    }
    #endregion
}