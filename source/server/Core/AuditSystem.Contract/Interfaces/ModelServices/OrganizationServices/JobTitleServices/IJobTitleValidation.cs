using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;

public interface IJobTitleValidation
{
    Task<JobTitle> ValidateJobTitleOwnershipAsync(Guid jobTitleId, Guid userId);
    Task<Company> ValidateCompanyOwnershipAsync(Guid jobTitleId, Guid companyId);
    Task ValidateJobTitleCreationAsync(Guid userId, Guid companyId);
}