using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditUniverseService(
    IRepository<Guid, AuditUniverse> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditUniverseService
{
    public async Task<Guid> CreateAuditUniverseAsync(AuditUniverseModel auditUniverseModel)
    {
        ArgumentNullException.ThrowIfNull(auditUniverseModel, nameof(auditUniverseModel));
        
        try
        {
            var entity = mapper.Map<AuditUniverse>(auditUniverseModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
