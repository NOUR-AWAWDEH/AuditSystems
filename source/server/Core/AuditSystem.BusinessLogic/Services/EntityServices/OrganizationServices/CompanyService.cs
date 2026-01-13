using AuditSystem.Application.Constants;
using AuditSystem.Application.Constants.Organization.CompanyConst;
using AuditSystem.BusinessLogic.Services.CacheServices;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.CompanyServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganizationServices;

internal sealed class CompanyService(
    IRepository<Guid, Company> repository,
    IRepository<Guid, CompanyUser> companyUserRepository,
    RoleManager<Role> roleManager,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<CompanyService> _logger,
    ICompanyValidation validationHelper,
    ICompanySearch searchHelper) :
    UnifiedCacheService<Guid, Company, CompanyModel>(
        repository,
        mapper,
        cacheService,
        _logger,
        CacheKeys.Company,
        CompanyConstants.CompanyTags,
        CompanyConstants.ListTags
    ),
    ICompanyService
{
    private readonly ICompanyValidation _validationHelper = validationHelper;
    private readonly ICompanySearch _searchHelper = searchHelper;

    public async Task<Guid> CreateCompanyAsync(CompanyModel companyModel, Guid createdByUserId)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));
        if (createdByUserId == Guid.Empty) throw new ArgumentException(CompanyConstants.UserIdEmptyMessage, nameof(createdByUserId));

        _logger.LogInformation("Creating company {CompanyName} for user {UserId}", companyModel.Name, createdByUserId);

        var entity = mapper.Map<Company>(companyModel);
        var createdEntity = await repository.CreateAsync(entity);

        var ownerRole = await roleManager.FindByNameAsync(Role.Admin)
            ?? throw new InvalidOperationException($"Role '{Role.Admin}' not found.");

        var companyUser = new CompanyUser
        {
            Id = Guid.NewGuid(),
            CompanyId = createdEntity.Id,
            AppUserId = createdByUserId,
            RoleId = ownerRole.Id,
            IsActive = true,
            CreatedByUserId = createdByUserId
        };

        await companyUserRepository.CreateAsync(companyUser);

        await CacheEntityAsync(entity);

        _logger.LogInformation("Successfully created company {CompanyId} for user {UserId}", createdEntity.Id, createdByUserId);

        return createdEntity.Id;
    }

    public async Task DeleteCompanyAsync(Guid companyId, Guid userId)
    {
        if (userId == Guid.Empty)
            throw new ArgumentException(CompanyConstants.UserIdEmptyMessage, nameof(userId));

        _logger.LogInformation("Deleting company {CompanyId} for user {UserId}", companyId, userId);

        await _validationHelper.ValidateCompanyOwnershipAsync(companyId, userId);

        // REQUIRED because of DeleteBehavior.Restrict
        await companyUserRepository.DeleteWhereAsync(cu => cu.CompanyId == companyId);

        await repository.DeleteAsync(companyId);
        await ClearEntityCacheAsync(companyId);

        _logger.LogInformation("Successfully deleted company {CompanyId} for user {UserId}", companyId, userId);
    }


    public async Task<CompanyModel> GetCompanyByIdAsync(Guid id, Guid userId)
    {
        if (userId == Guid.Empty) throw new ArgumentException(CompanyConstants.UserIdEmptyMessage, nameof(userId));

        _logger.LogDebug("Retrieving company {CompanyId} for user {UserId}", id, userId);

        var entity = await _validationHelper.ValidateCompanyOwnershipAsync(id, userId);

        // Strip CompanyUsers to prevent caching stale relation data
        entity.CompanyUsers = [];

        await CacheEntityAsync(entity);
        return mapper.Map<CompanyModel>(entity);
    }

    public async Task<PaginationResult<CompanyModel>> SearchAsync(string? name, Page page)
    {
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching companies with name filter: {Name}", name ?? "(none)");

        var (cacheKey, countCacheKey) = GetSearchCacheKeys(name, page);

        var cachedResult = await GetFromCacheAsync<CompanyModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(CompanyConstants.RetrievedCompanySearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await GetFromRepositoryAsync<CompanyModel>(cacheKey, countCacheKey, page, name);
    }

    public async Task<PaginationResult<CompanyModel>> SearchByUserIdAsync(Guid userId, string? searchTerm, Page page)
    {
        if (userId == Guid.Empty) throw new ArgumentException(CompanyConstants.UserIdEmptyMessage, nameof(userId));
        ArgumentNullException.ThrowIfNull(page, nameof(page));

        _logger.LogDebug("Searching companies for user {UserId} with term: {SearchTerm}", userId, searchTerm ?? "(none)");

        var (cacheKey, countCacheKey) = _searchHelper.GetUserSearchCacheKeys(userId, searchTerm, page);

        var cachedResult = await GetFromCacheAsync<CompanyModel>(cacheKey, countCacheKey, page);
        if (cachedResult != null)
        {
            _logger.LogDebug(CompanyConstants.RetrievedUserSearchResultsFromCacheMessage);
            return cachedResult;
        }

        return await _searchHelper.GetUserSearchFromRepositoryAsync(userId, searchTerm, page, cacheKey, countCacheKey);
    }

    public async Task UpdateCompanyAsync(CompanyModel companyModel, Guid userId)
    {
        ArgumentNullException.ThrowIfNull(companyModel, nameof(companyModel));
        if (userId == Guid.Empty) throw new ArgumentException(CompanyConstants.UserIdEmptyMessage, nameof(userId));

        _logger.LogInformation("Updating company {CompanyId} for user {UserId}", companyModel.Id, userId);

        var existingEntity = await _validationHelper.ValidateCompanyOwnershipAsync(companyModel.Id, userId);

        mapper.Map(companyModel, existingEntity);
        existingEntity.UpdatedAt = DateTime.UtcNow; // Base entity usually handles this or interceptor

        await repository.UpdateAsync(existingEntity);
        await CacheEntityAsync(existingEntity);

        _logger.LogInformation("Successfully updated company {CompanyId}", companyModel.Id);
    }
}