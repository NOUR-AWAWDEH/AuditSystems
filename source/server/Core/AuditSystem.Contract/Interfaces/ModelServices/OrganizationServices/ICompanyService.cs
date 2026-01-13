using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

public interface ICompanyService
{
    Task<Guid> CreateCompanyAsync(CompanyModel companyModel, Guid createdByUserId);
    Task UpdateCompanyAsync(CompanyModel companyModel, Guid userId);
    Task DeleteCompanyAsync(Guid companyId, Guid userId);
    Task<CompanyModel> GetCompanyByIdAsync(Guid id, Guid userId);
    Task<PaginationResult<CompanyModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page page);
    Task<PaginationResult<CompanyModel>> SearchAsync(string? name, Page page);
}