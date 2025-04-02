namespace AuditSystem.Domain.Entities.Common;

public class Rating : Entity<Guid>
{
    public string Level { get; set; } = string.Empty;
    public static readonly string[] ValidLevels = { "High", "Medium", "Low", "Critical", "Minimal" };
}
