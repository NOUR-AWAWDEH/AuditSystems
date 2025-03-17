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
    public Task<Guid> CreateSkillSetAsync(SkillSetModel skillSetModel)
    {
        throw new NotImplementedException();
    }
}