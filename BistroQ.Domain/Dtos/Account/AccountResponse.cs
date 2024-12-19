using BistroQ.Domain.Dtos.Tables;

namespace BistroQ.Domain.Dtos.Account;

public class AccountResponse
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public TableResponse? Table { get; set; }
}
