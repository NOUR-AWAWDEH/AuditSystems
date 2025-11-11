using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organization;

public sealed class LocationModel : BaseModel<Guid>
{
    public required string Name { get; set; }
    public string? Address { get; set; } = string.Empty;
    public required Guid CompanyId { get; set; }
    public Guid UserId { get; set; } = Guid.Empty;
}
