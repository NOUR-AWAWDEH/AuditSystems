using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class CompanyService(
    IRepository<Guid, Company> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<CompanyService> logger) : 
    BaseEntityCacheService<Company>(
        cacheService,
        logger,
        CacheKeys.Company,
        CompanyTags,
        ListTags
    ),
    ICompanyService
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

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for CompanyModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create Company");
            throw new InvalidOperationException("Failed to create Company.", ex);
        }
    }

    public async Task DeleteCompanyAsync(Guid companyId)
    {
        var cacheKey = GetCacheKey(companyId);
        
        try
        {
            var entity = await repository.GetByIdAsync(companyId)
                ?? throw new KeyNotFoundException($"Company with ID {companyId} not found.");

            await repository.DeleteAsync(companyId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "Company with ID {CompanyId} not found", companyId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete Company ID {CompanyId}", companyId);
            throw new InvalidOperationException($"Failed to delete Company: {companyId}", ex);
        }
    }

    public async Task<CompanyModel> GetCompanyByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var cachedEntity = await cacheService.GetAsync<Company>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<CompanyModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"Company with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<CompanyModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve Company with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve Company with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<CompanyModel>> SearchAsync(string? name, Page page)
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
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update Company with ID {Id}", companyModel.Id);
            throw new InvalidOperationException($"Failed to update Company with ID '{companyModel.Id}'", ex);
        }
    }
}