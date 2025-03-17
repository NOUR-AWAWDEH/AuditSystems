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
    public Task<Guid> CreateAuditEngagementAsync(AuditEngagementModel auditEngagementModel)
    {
        throw new NotImplementedException();
    }
}
