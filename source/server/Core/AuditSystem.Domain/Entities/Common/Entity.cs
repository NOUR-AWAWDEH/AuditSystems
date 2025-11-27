namespace AuditSystem.Domain.Entities.Common;

public abstract class Entity<TId>
where TId : IComparable<TId>
{
    public required TId Id { get; set; }
    
}
