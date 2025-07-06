using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Contract.Models.Skills;

public class SkillSetModel : BaseModel<Guid>
{
    public required string ProficiencyLevel { get; set; } = string.Empty;
}
