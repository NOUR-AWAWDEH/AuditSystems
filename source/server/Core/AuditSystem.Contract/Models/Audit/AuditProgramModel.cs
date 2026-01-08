using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Users;

namespace AuditSystem.Contract.Models.Audit;

public class AuditProgramModel : BaseModel<Guid>
{
    public required string Description { get; set; }
    public required Guid ExitControlId { get; set; }
    public required Guid ProcessOwnerId { get; set; }
    public Guid? ControlDesignId { get; set; } = Guid.Empty;
    
    public UserModel? ProcessOwner { get; set; }
}