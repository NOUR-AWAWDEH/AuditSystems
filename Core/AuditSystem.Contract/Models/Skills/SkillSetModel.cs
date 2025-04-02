using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Skills;

public class SkillSetModel : BaseModel<Guid>
{
    public required Guid UserManagementId { get; set; }
    public required Guid SkillId { get; set; }
    public string ProficiencyLevel { get; set; } = string.Empty;
}
