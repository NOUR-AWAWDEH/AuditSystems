namespace AuditSystem.Domain.Entities.Common;

public class Rate : Entity<Guid>
{
    public required string Level { get; set; } = string.Empty;
}