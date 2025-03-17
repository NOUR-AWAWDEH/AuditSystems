using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;

internal sealed class ComplianceChecklistService(
    IRepository<Guid, ComplianceChecklist> repository,
    IMapper mapper,
    ICacheService cacheService
    )
    : IComplianceChecklistService
{
    public Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        throw new NotImplementedException();
    }
}
