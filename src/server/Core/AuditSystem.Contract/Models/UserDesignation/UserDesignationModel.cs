using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.UserDesignation;

public sealed class UserDesignationModel : BaseModel<Guid>
{
    public required string Designation { get; set; } = string.Empty;
    public required string Level { get; set; }
    public required bool IsActive { get; set; } = true;
    public required Guid UserId { get; set; }
    public string Description { get; set; } = string.Empty;
}