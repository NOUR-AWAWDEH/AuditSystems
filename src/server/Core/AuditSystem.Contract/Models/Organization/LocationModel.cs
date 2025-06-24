using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organization;

public sealed class LocationModel : BaseModel<Guid>
{
    public required string Name { get; set; } = string.Empty;
    public string? Address { get; set; }
    public required Guid CompanyId { get; set; }
    public required Guid UserId { get; set; }
}
