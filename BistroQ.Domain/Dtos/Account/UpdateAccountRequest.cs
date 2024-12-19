﻿namespace BistroQ.Domain.Dtos.Account;

public class UpdateAccountRequest
{
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? Role { get; set; }
    public int? TableId { get; set; }
}