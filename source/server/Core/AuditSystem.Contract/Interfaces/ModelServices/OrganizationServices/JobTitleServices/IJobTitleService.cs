using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;

public interface IJobTitleService
{
    Task<PaginationResult<JobTitleModel>> SearchByCompanyIdAsync(string? searchTerm, Guid companyId, Page page,Guid userId);
}