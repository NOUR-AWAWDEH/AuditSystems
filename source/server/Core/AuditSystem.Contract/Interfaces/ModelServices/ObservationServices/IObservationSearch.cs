using System.Linq.Expressions;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Observations;

namespace AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;

public interface IObservationSearch
{
    Expression<Func<Observation, bool>> BuildUserSearchPredicate(Guid userId, string? searchTerm, Guid auditProgramId);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid auditProgramId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Guid? auditProgramId, Page page);
    (string cacheKey, string countCacheKey) GetUserSearchCacheKeys(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<ObservationModel>> GetUserSearchFromRepositoryAsync(Guid userId, string? searchTerm, Guid auditProgramId, Page page, string cacheKey, string countCacheKey);
}