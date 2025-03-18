using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlService(
    IRepository<Guid, RiskControl> repository,
    IMapper mapper,
    ICacheService cacheService) 
    : IRiskControlService
{
    public async Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel)
    {
        ArgumentNullException.ThrowIfNull(riskControlModel, nameof(riskControlModel));
        
        try
        {
            var entity = mapper.Map<RiskControl>(riskControlModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
