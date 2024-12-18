namespace BistroQ.Domain.Dtos.Account;

public class AccountResponse
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public int? ZoneId { get; set; }

    public int? TableId { get; set; }

    public int? TableNumber { get; set; }

    public string? ZoneName { get; set; }
}
