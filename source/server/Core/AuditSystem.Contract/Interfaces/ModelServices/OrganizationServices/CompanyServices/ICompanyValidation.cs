using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.CompanyServices;

public interface ICompanyValidation
{
    Task<Company> ValidateCompanyOwnershipAsync(Guid companyId, Guid userId);
}
