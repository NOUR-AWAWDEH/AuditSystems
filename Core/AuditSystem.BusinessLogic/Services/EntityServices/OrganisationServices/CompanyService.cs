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
    public async Task<Guid> CreateCompanyAsync(CompanyModel companyModel)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));
        
        try
        {
            var entity = mapper.Map<Company>(companyModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}