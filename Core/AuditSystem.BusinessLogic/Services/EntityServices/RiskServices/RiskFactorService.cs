using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskFactorService(
    IRepository<Guid, RiskFactor> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskFactorService
{
    public async Task<Guid> CreateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));
        
        try
        {
            var entity = mapper.Map<RiskFactor>(riskFactorModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}