namespace AuditSystem.Domain.Entities.Common;

public  class Rating : Entity<Guid>
{
    public required string Level { get; set; } = string.Empty;
}