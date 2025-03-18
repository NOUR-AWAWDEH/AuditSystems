using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskAssessmentService(
    IRepository<Guid, RiskAssessment> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskAssessmentService
{
    public async Task<Guid> CreateRiskAssessmentAsync(RiskAssessmentModel riskAssessmentModel)
    {
        ArgumentNullException.ThrowIfNull(riskAssessmentModel, nameof(riskAssessmentModel));
        
        try
        {
            var entity = mapper.Map<RiskAssessment>(riskAssessmentModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
