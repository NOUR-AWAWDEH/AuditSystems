using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ICompanyService
{
    public Task<Guid> CreateCompanyAsync(CompanyModel companyModel);
    public Task UpdateCompanyAsync(CompanyModel companyModel);
    public Task DeleteCompanyAsync(Guid companyId);
    public Task<CompanyModel> GetCompanyByIdAsync(Guid Id);
}