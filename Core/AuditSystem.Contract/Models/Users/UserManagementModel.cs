using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Users;

public sealed class UserManagementModel : BaseModel<Guid>
{
    public required Guid AuditorSettingsID { get; set; }
    public required Guid UserID { get; set; }
    public required string Designation { get; set; } = string.Empty;
}
