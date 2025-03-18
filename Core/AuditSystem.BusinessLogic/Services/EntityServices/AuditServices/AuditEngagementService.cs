using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditEngagementService(
    IRepository<Guid, AuditEngagement> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditEngagementService
{
    public async Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        ArgumentNullException.ThrowIfNull(auditEngagementModel, nameof(auditEngagementModel));
        
        try 
        {
            var entity = mapper.Map<AuditEngagement>(auditEngagementModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex) {
        
        }
        throw new NotImplementedException();
    }
}
