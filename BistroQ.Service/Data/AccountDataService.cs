using AutoMapper;
using BistroQ.Domain.Contracts.Services;
using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Models.Entities;

namespace BistroQ.Service.Data;

public class AccountDataService : IAccountDataService
{
    private readonly IApiClient _apiClient;
    private readonly IMapper _mapper;

    public AccountDataService(IApiClient apiClient, IMapper mapper)
    {
        _apiClient = apiClient;
        _mapper = mapper;
    }

    public async Task<ApiCollectionResponse<IEnumerable<Account>>> GetGridDataAsync(AccountCollectionQueryParams query = null)
    {
        //await Task.Delay(2000);
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
}