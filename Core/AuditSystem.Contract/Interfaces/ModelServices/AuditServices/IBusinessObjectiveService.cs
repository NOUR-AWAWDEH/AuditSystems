using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

public interface IBusinessObjectiveService
{
    public Task<Guid> CreateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel);
    public Task UpdateBusinessObjectiveAsync(BusinessObjectiveModel businessObjectiveModel);
    public Task DeleteBusinessObjectiveAsync(Guid businessObjectiveId);
    public Task<BusinessObjectiveModel> GetBusinessObjectiveByIdAsync(Guid id);
    public Task<PaginationResult<BusinessObjectiveModel>> Search(string? Name, Page page);
}