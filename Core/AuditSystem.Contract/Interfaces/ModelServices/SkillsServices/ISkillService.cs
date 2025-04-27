using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Skills;

namespace AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

public interface ISkillService
{
    public Task<Guid> CreateSkillAsync(SkillModel skillModel);
    public Task UpdateSkillAsync(SkillModel skillModel);
    public Task DeleteSkillAsync(Guid skillId);
    public Task<SkillModel> GetSkillByIdAsync(Guid id);
    public Task<PaginationResult<SkillModel>> SearchAsync(string? name, Page page);
}
