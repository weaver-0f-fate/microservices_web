﻿namespace Subscription.Domain.Exceptions;
public record ErrorResult
{
    public List<string> Messages { get; set; } = new();
    public string? Source { get; set; }
    public string? Exception { get; set; }
    public string? ErrorId { get; set; }
    public string? SupportMessage { get; set; }
    public int StatusCode { get; set; }
}