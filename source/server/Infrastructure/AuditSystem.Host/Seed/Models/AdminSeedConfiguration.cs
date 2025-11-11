namespace AuditSystem.Host.Seed.Models;

public sealed record AdminSeedConfiguration
{
    public required string Email { get; init; }
    public required string Password { get; init; }
    public required string Username { get; init; }
    public required Guid CompanyId { get; init; }
}