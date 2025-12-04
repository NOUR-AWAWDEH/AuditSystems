using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Users;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;

public interface IJobTitleValidation
{
    Task<JobTitle> ValidateJobTitleOwnershipAsync(Guid jobTitleId, Guid userId);
    Task<Company> ValidateCompanyOwnershipAsync(Guid jobTitleId, Guid companyId);
    Task ValidateJobTitleCreationAsync(Guid userId, Guid companyId);
    Task<AppUser> ValidateUserAsync(Guid userId);
    Task ValidateJobTitleNameUniquenessAsync(string name, Guid companyId);
    Task ValidateJobTitleNameUniquenessAsync(string name, Guid companyId, Guid excludeJobTitleId);
}