using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

internal sealed class CompanyService(
    IRepository<Guid, Company> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ICompanyService
{
    public Task<Guid> CreateCompanyAsync(CompanyModel companyModel)
    {
        throw new NotImplementedException();
    }
}