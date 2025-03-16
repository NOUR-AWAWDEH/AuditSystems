using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Contract.Interfaces.ModelServices.RisksServices;

public interface ISpecificRiskFactorService
{
    public Task<Guid> CreateSpecificRiskFactorAsync(SpecificRiskFactorModel specificRiskFactorModel);
}
