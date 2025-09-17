using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Risks;

public class ControlDesignModel : BaseModel<Guid>
{
    public required string Description { get; set; }
    public required Guid ExitControlId { get; set; }
    public required bool Inadequate { get; set; } = true;
}
