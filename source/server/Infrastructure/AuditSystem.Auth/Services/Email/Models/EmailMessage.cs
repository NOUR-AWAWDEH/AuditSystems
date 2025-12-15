namespace AuditSystem.Auth.Services.Email.Models;

public sealed class EmailMessage
{
    public required List<string> To { get; init; }
    public required string Subject { get; init; }
    public required string Body { get; init; }
    public bool IsHtml { get; init; } = true;
}
