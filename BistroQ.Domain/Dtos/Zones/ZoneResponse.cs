﻿namespace BistroQ.Domain.Dtos.Zones;

public class ZoneResponse
{
    public int? ZoneId { get; set; }

    public string? Name { get; set; }

    public int? TableCount { get; set; }

    public bool? HasCheckingOutTables { get; set; }
}