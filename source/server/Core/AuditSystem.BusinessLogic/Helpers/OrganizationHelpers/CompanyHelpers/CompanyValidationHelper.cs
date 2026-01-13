using AuditSystem.Application.Constants.Organization.CompanyConst;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.CompanyServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Organization;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.BusinessLogic.Helpers.OrganizationHelpers.CompanyHelpers;

public class CompanyValidationHelper(
    IRepository<Guid, Company> repository)
    : ICompanyValidation
{
    private readonly IRepository<Guid, Company> _repository = repository;

    public async Task<Company> ValidateCompanyOwnershipAsync(Guid companyId, Guid userId)
    {
        var company = await _repository.Query()
            .Include(c => c.CompanyUsers)
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == companyId);

        if (company == null)
        {
            throw new KeyNotFoundException(string.Format(CompanyConstants.CompanyNotFoundMessage, companyId));
        }

        // Check if user is associated with the company
        var isAssociated = company.CompanyUsers.Any(cu => cu.AppUserId == userId);

        if (!isAssociated)
        {
            throw new UnauthorizedAccessException(
                string.Format(CompanyConstants.UnauthorizedCompanyAccessMessage, userId, companyId)
            );
        }

        return company;
    }
}
