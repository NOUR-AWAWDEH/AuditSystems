using System.Linq.Expressions;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;

public interface IJobTitleSearch
{
    Expression<Func<JobTitle, bool>> BuildCompanySearchPredicate(Guid companyId, string? searchTerm);
    (string cacheKey, string countCacheKey) GetCompanySearchCacheKeys(Guid companyId, string? searchTerm, Page page);
    Task<PaginationResult<JobTitleModel>> GetCompanySearchFromRepositoryAsync(Guid companyId, string? searchTerm, Page page, string cacheKey, string countCacheKey);
}