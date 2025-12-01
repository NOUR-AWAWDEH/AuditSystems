using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public class AuditProgramModel : BaseModel<Guid>
{
    public required string Description { get; set; }
    public required Guid ExitControlId { get; set; }
    public required Guid ActionOwnerId { get; set; }
    public Guid? ControlDesignId { get; set; } = Guid.Empty;
}