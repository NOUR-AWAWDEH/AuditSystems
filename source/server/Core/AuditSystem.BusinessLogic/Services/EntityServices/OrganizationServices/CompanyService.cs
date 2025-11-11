using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AutoMapper;
using Microsoft.Extensions.Logging;
using AuditSystem.BusinessLogic.Services.CacheServices;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class CompanyService(
    IRepository<Guid, Company> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<CompanyService> _logger) : 
    UnifiedCacheService<Guid, Company, CompanyModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Company,
        CompanyTags,
        ListTags
    ),
    ICompanyService
{
    private static readonly string[] CompanyTags = ["companies", "company-list"];
    private static readonly string[] ListTags = ["company-list"];

    public async Task<Guid> CreateCompanyAsync(CompanyModel companyModel)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));

        var entity = mapper.Map<Company>(companyModel);
        var createdEntity = await repository.CreateAsync(entity);
       await CacheEntityAsync(entity);

        return createdEntity.Id;
    }

    public async Task DeleteCompanyAsync(Guid companyId)
    {        
        var entity = await repository.GetByIdAsync(companyId)
            ?? throw new KeyNotFoundException($"Company with ID {companyId} not found.");

        await repository.DeleteAsync(companyId);
       await CacheEntityAsync(entity);
    }

    public async Task<CompanyModel> GetCompanyByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        var cachedEntity = await cacheService.GetAsync<Company>(cacheKey);
        if (cachedEntity != null)
            return mapper.Map<CompanyModel>(cachedEntity);

        var entity = await repository.GetByIdAsync(id)
            ?? throw new KeyNotFoundException($"Company with ID '{id}' not found.");

       await CacheEntityAsync(entity);
        
        return mapper.Map<CompanyModel>(entity);
    }

    public async Task<PaginationResult<CompanyModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));
        
        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<CompanyModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
            return cachedResult;

        return await GetFromRepositoryAsync<CompanyModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task UpdateCompanyAsync(CompanyModel companyModel)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));
    
        var entity = mapper.Map<Company>(companyModel);
        await repository.UpdateAsync(entity);
        
        await CacheEntityAsync(entity);
    }
}