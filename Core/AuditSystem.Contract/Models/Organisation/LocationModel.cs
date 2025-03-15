using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Organisation;

public sealed class LocationModel : BaseModel<Guid>
{
    public required Guid AuditorSettingsId { get; set; }
    public required  string LocationName { get; set; } = string.Empty;
}
