using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillService(
    IRepository<Guid, Skill> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISkillService
{
    public async Task<Guid> CreateSkillAsync(SkillModel skillModel)
    {
        ArgumentNullException.ThrowIfNull(skillModel, nameof(skillModel));

        try
        {
            var entity = mapper.Map<Skill>(skillModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
