using AuditSystem.Application.Constants.Organization.CompanyConst;
using AuditSystem.Application.Constants.Organization.JobTitleConst;
using AuditSystem.BusinessLogic.Utilities;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.JobTitleHelpers;

public class JobTitleValidationHelper(
    IRepository<Guid, JobTitle> repository,
    IRepository<Guid, Company> companyRepository)
    : IJobTitleValidation
{
    private readonly IRepository<Guid, JobTitle> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Company> _companyRepository = companyRepository
        ?? throw new ArgumentNullException(nameof(companyRepository));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException(JobTitleConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<JobTitle> ValidateJobTitleOwnershipAsync(Guid jobTitleId, Guid companyId)
    {
        return await OwnershipValidator.ValidateOwnershipAsync<JobTitle, Guid>(
             _repository.Query(),
             d => d.Id == jobTitleId,
             JobTitleConstants.JobTitleCompanyIdSelector,
             companyId,
             string.Format(JobTitleConstants.JobTitleNotFoundMessage, jobTitleId),
             string.Format(JobTitleConstants.UnauthorizedAccessMessage, companyId, jobTitleId)
         );
    }

    public async Task<Company> ValidateCompanyOwnershipAsync(Guid jobTitleId, Guid companyId)
    {
        var company = await _companyRepository.GetByIdAsync(companyId)
           ?? throw new ArgumentException(CompanyConstants.CompanyNotFoundMessage, nameof(companyId));

        var isRelated = await _repository.Query()
            .AsNoTracking()
            .AnyAsync(j => j.Id == jobTitleId && j.CompanyId == companyId);

        if (!isRelated)
        {
            throw new UnauthorizedAccessException(
                string.Format(CompanyConstants.UnauthorizedAccessMessage, jobTitleId, companyId)
            );
        }

        return company;
    }
}