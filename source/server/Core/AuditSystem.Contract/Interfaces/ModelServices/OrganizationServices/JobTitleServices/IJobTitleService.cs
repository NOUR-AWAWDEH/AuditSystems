using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;

public interface IJobTitleService
{
    Task<Guid> CreateJobTitleAsync(JobTitleModel model, Guid userId);
    Task UpdateJobTitleAsync(JobTitleModel model, Guid userId);
    Task DeleteJobTitleAsync(Guid id, Guid userId);
    Task<JobTitleModel> GetJobTitleByIdAsync(Guid id, Guid userId);
    Task<IReadOnlyList<JobTitleModel>> GetJobTitlesByUserIdAsync(Guid userId);
    Task<PaginationResult<JobTitleModel>> SearchByUserIdAsync(string? searchTerm, Page page, Guid userId, Guid? companyId);
}