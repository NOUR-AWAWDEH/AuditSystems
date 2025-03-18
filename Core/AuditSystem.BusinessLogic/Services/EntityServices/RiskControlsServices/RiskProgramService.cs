using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskProgramService(
    IRepository<Guid, RiskProgram> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskProgramService
{
    public async Task<Guid> CreateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));
        
        try
        {
            var entity = mapper.Map<RiskProgram>(riskProgramModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
