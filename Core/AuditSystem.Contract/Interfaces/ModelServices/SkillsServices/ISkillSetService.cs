using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

public interface ISkillSetService
{
    public Task<Guid> CreateSkillSetAsync(SkillSetModel skillSetModel);
}