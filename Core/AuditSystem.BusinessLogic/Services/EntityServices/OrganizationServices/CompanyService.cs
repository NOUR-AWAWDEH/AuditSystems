using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

internal sealed class CompanyService(
    IRepository<Guid, Company> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ICompanyService
{
    private static readonly string[] CompanyTags = ["companies", "company-list"];
    private static readonly string[] ListTags = ["company-list"]; // Tags for collections only

    public async Task<Guid> CreateCompanyAsync(CompanyModel companyModel)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));

        try
        {
            var entity = mapper.Map<Company>(companyModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.Company, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: CompanyTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;

        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create Company.", ex);
        }
    }

    public async Task DeleteCompanyAsync(Guid companyId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(companyId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"Company with ID {companyId} not found.");
            }
            
            await repository.DeleteAsync(companyId);
            
            var cacheKey = string.Format(CacheKeys.Company, companyId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete Company.", ex);
        }
    }

    public Task<CompanyModel> GetCompanyByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateCompanyAsync(CompanyModel companyModel)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));

        try
        {
            var entity = mapper.Map<Company>(companyModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.Company, entity.Id);
        
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: CompanyTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update Company.", ex);
        }
    }
}