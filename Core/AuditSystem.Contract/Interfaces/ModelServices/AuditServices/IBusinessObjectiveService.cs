using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IBusinessObjectiveService
{
    public Task<Guid> CreateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel);
}
