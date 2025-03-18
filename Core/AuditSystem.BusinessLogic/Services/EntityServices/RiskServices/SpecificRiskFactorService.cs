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
    public async Task<Guid> CreateSpecificRiskFactorAsync(SpecificRiskFactorModel specificRiskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(specificRiskFactorModel, nameof(specificRiskFactorModel));
        
        try
        {
            var entity = mapper.Map<SpecificRiskFactor>(specificRiskFactorModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}