using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.CommonServices;

public interface IRateService
{
    Task<Guid> CreateRateAsync(RateModel rateModel);
    Task UpdateRateAsync(RateModel rateModel);
    Task DeleteRateAsync(Guid rateId);
    Task<RateModel> GetRateByIdAsync(Guid id);
    Task<PaginationResult<RateModel>> SearchAllAsync(Page page);
}