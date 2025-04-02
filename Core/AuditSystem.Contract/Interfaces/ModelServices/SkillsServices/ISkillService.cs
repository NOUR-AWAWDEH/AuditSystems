using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

public interface ISkillService
{
    public Task<Guid> CreateSkillAsync(SkillModel skillModel);
}
