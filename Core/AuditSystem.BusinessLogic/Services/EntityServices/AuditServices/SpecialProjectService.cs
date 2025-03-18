using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class SpecialProjectService(
    IRepository<Guid, SpecialProject> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISpecialProjectService
{
    public async Task<Guid> CreateSpecialProjectAsync(SpecialProjectModel specialProjectModel)
    {
        ArgumentNullException.ThrowIfNull(specialProjectModel, nameof(specialProjectModel));
        
        try
        {
            var entity = mapper.Map<SpecialProject>(specialProjectModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}