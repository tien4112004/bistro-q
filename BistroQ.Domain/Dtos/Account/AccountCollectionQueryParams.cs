namespace BistroQ.Domain.Dtos.Account;

public class AccountCollectionQueryParams : BaseCollectionQueryParams
{
    public string? Username { get; set; }
    public string? Role { get; set; }
}
