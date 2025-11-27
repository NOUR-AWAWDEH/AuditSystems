using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Domain.Entities.Common;

public abstract class AuditableEntity<T> : Entity<T> where T : IComparable<T>
{
    public Guid CreatedByUserId { get; set; }
    public virtual AppUser CreatedBy { get; set; } = null!;
    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}
