using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class SpecificRiskFactorService(
    IRepository<Guid, SpecificRiskFactor> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISpecificRiskFactorService
{
    public Task<Guid> CreateSpecificRiskFactorAsync(SpecificRiskFactorModel specificRiskFactorModel)
    {
        throw new NotImplementedException();
    }
}