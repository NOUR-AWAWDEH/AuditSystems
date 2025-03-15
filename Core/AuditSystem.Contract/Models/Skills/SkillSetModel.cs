using AuditSystem.Contract.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditSystem.Contract.Models.Skills;

public class SkillSetModel : BaseModel<Guid>
{
    public required Guid UserManagementId { get; set; }
    public required Guid SkillId { get; set; }
    public string ProficiencyLevel { get; set; } = string.Empty;
}
