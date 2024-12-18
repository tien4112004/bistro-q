namespace BistroQ.Domain.Models.Entities;

public class Account
{
    public string Id { get; set; }

    public string Username { get; set; }

    public string Role { get; set; }

    public int? TableId { get; set; }

    public Table Table { get; set; }
}
