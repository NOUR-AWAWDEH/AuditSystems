using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Audit;

public class AuditProgramModel : BaseModel<Guid> 
{
    public required string Description { get; set; } = string.Empty;
    public required Guid ExitControlId { get; set; } = Guid.Empty;
    public Guid? ControlDesignId { get; set; } = Guid.Empty;
}