using BistroQ.Domain.Dtos;
using BistroQ.Domain.Dtos.Account;
using BistroQ.Domain.Models.Entities;

public interface IAccountDataService
{
    Task<ApiCollectionResponse<IEnumerable<Account>>> GetGridDataAsync(AccountCollectionQueryParams query);
    Task<Account> GetAccountByIdAsync(string userId);
    Task<Account> CreateAccountAsync(CreateAccountRequest request);
    Task<Account> UpdateAccountAsync(string userId, UpdateAccountRequest request);
    Task<bool> DeleteAccountAsync(string userId);
}
