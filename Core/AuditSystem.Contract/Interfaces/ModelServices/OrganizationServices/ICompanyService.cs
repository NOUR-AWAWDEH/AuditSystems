using AuditSystem.Contract.Models.Organisation;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;

public interface ICompanyService
{
    public Task<Guid> CreateCompanyAsync(CompanyModel companyModel);
    public Task UpdateCompanyAsync(CompanyModel companyModel);
}