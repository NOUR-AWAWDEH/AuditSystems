using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskControlMatrixService(
    IRepository<Guid, RiskControlMatrix> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskControlMatrixService
{
    public Task<Guid> CreateRiskControlMatrixAsync(RiskControlMatrixModel riskControlMatrixModel)
    {
        throw new NotImplementedException();
    }
}
