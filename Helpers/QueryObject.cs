﻿namespace api.Helpers;

public class QueryObject
{
    public string? Title { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 6;

}