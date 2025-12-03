using AuditSystem.Application.Constants.Organization.CompanyConst;
using AuditSystem.Application.Constants.Organization.JobTitleConst;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.JobTitleHelpers;

public class JobTitleValidationHelper(
    IRepository<Guid, JobTitle> repository,
    IRepository<Guid, Company> companyRepository,
    IRepository<Guid, AppUser> appUserRepository,
    UserManager<ApplicationUser> userManager)
    : IJobTitleValidation
{
    private readonly IRepository<Guid, JobTitle> _repository = repository
        ?? throw new ArgumentNullException(nameof(repository));
    private readonly IRepository<Guid, Company> _companyRepository = companyRepository
        ?? throw new ArgumentNullException(nameof(companyRepository));
    private readonly IRepository<Guid, AppUser> _appUserRepository = appUserRepository
        ?? throw new ArgumentNullException(nameof(appUserRepository));
    private readonly UserManager<ApplicationUser> _userManager = userManager
        ?? throw new ArgumentNullException(nameof(userManager));

    public static void ValidateUserId(Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException(JobTitleConstants.UserIdEmptyMessage, nameof(userId));
    }

    public async Task<JobTitle> ValidateJobTitleOwnershipAsync(Guid jobTitleId, Guid userId)
    {
        var appUser = await _appUserRepository.Query()
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.IdentityUserId == userId);

        if (appUser == null)
            throw new UnauthorizedAccessException(string.Format(JobTitleConstants.UnauthorizedAccessMessage, userId, jobTitleId));

        var jobTitle = await _repository.Query()
            .Include(j => j.Users)
            .FirstOrDefaultAsync(j => j.Id == jobTitleId);

        if (jobTitle == null)
            throw new KeyNotFoundException(string.Format(JobTitleConstants.JobTitleNotFoundMessage, jobTitleId));

        if (jobTitle.CompanyId != appUser.CompanyId)
        {
            throw new UnauthorizedAccessException(string.Format(JobTitleConstants.UnauthorizedAccessMessage, userId, jobTitleId));
        }

        var ownerId = jobTitle.Users.Select(u => u.IdentityUserId).FirstOrDefault();
        if (ownerId != userId)
        {
            throw new UnauthorizedAccessException(string.Format(JobTitleConstants.UnauthorizedAccessMessage, userId, jobTitleId));
        }

        return jobTitle;
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
    public async Task ValidateJobTitleCreationAsync(Guid userId, Guid companyId)
    {
        var appUser = await _appUserRepository.Query()
            .FirstOrDefaultAsync(u => u.IdentityUserId == userId);

        if (appUser == null)
        {
            throw new UnauthorizedAccessException($"User with ID {userId} not found.");
        }

        if (appUser.CompanyId != companyId)
        {
            throw new UnauthorizedAccessException($"User {userId} is not authorized to create job titles for company {companyId}.");
        }

        var identityUser = await _userManager.FindByIdAsync(userId.ToString());
        if (identityUser == null || !await _userManager.IsInRoleAsync(identityUser, Role.Admin))
        {
            throw new UnauthorizedAccessException($"User {userId} does not have Admin privileges.");
        }
    }
}