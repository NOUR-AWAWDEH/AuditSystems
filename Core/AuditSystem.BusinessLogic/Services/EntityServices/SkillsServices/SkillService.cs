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
    public Task<Guid> CreateSkillAsync(SkillModel skillModel)
    {
        throw new NotImplementedException();
    }
}
