namespace AuditSystem.Domain.Entities.Common;

public abstract class Entity<TId>
where TId : IComparable<TId>
{
    public required TId Id { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
