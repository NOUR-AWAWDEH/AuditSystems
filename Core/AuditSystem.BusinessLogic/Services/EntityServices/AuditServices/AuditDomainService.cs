using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.AuditServices;

internal sealed class AuditDomainService(
    IRepository<Guid, AuditDomain> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<AuditDomainService> logger
) : BaseEntityCacheService<AuditDomain>(
        cacheService,
        logger,
        CacheKeys.AuditDomain,
        AuditDomainTags,
        ListTags
    ),
    IAuditDomainService
{
    private static readonly string[] AuditDomainTags = ["audit-domains", "audit-domain-list"];
    private static readonly string[] ListTags = ["audit-domain-list"];

    public async Task<Guid> CreateAuditDomainAsync(AuditDomainModel auditDomainModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomainModel, nameof(auditDomainModel));

        try
        {
            var entity = mapper.Map<AuditDomain>(auditDomainModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for AuditDomainModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create AuditDomain");
            throw new InvalidOperationException("Failed to create AuditDomain.", ex);
        }
    }

    public async Task DeleteAuditDomainAsync(Guid auditDomainId)
    {
        var cacheKey = GetCacheKey(auditDomainId);

        try
        {
            var entity = await repository.GetByIdAsync(auditDomainId)
                         ?? throw new KeyNotFoundException($"AuditDomain with ID {auditDomainId} not found.");

            await repository.DeleteAsync(auditDomainId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogWarning(ex, "AuditDomain with ID {Id} not found", auditDomainId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete AuditDomain with ID {Id}", auditDomainId);
            throw new InvalidOperationException($"Failed to delete AuditDomain with ID {auditDomainId}", ex);
        }
    }

    public async Task<AuditDomainModel> GetAuditDomainByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);

        try
        {
            var cachedEntity = await cacheService.GetAsync<AuditDomain>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<AuditDomainModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id)
                         ?? throw new KeyNotFoundException($"AuditDomain with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);

            return mapper.Map<AuditDomainModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve AuditDomain with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve AuditDomain with ID '{id}'", ex);
        }
    }

    public Task<PaginationResult<AuditDomainModel>> Search(string? Name, Page page)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAuditDomainAsync(AuditDomainModel auditDomainModel)
    {
        ArgumentNullException.ThrowIfNull(auditDomainModel, nameof(auditDomainModel));

        try
        {
            var entity = mapper.Map<AuditDomain>(auditDomainModel);
            await repository.UpdateAsync(entity);

            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update AuditDomain with ID {Id}", auditDomainModel.Id);
            throw new InvalidOperationException("Failed to update AuditDomain.", ex);
        }
    }
}
