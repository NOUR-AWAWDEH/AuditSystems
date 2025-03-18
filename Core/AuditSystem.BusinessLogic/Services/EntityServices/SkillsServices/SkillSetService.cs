using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SkillsServices;

internal sealed class SkillSetService(
    IRepository<Guid, SkillSet> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISkillSetService
{
    public async Task<Guid> CreateSkillSetAsync(SkillSetModel skillSetModel)
    {
        ArgumentNullException.ThrowIfNull(skillSetModel, nameof(skillSetModel));

        try
        {
            var entity = mapper.Map<SkillSet>(skillSetModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}