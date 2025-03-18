using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.JobsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Jobs;
using AuditSystem.Domain.Entities.Jobs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.JobsServices;

internal sealed class AuditJobService(
    IRepository<Guid, AuditJob> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditJobService
{
    public async Task<Guid> CreateAuditJobAsync(AuditJobModel auditJobModel)
    {
        ArgumentNullException.ThrowIfNull(auditJobModel, nameof(auditJobModel));
        
        try
        {
            var entity = mapper.Map<AuditJob>(auditJobModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
