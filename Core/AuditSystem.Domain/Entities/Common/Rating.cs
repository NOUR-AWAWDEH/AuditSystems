namespace AuditSystem.Domain.Entities.Common;

public class Rating : Entity<Guid>
{
    public required string Level { get; set; } = string.Empty;
    public static readonly string[] ValidLevels = { "High", "Medium", "Low", "Critical", "Minimal" };
}