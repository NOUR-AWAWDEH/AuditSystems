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
    public Task<Guid> CreateRiskControlAsync(RiskControlsModel riskControlModel)
    {
        throw new NotImplementedException();
    }
}
