using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;

internal sealed class SupportingDocService(
    IRepository<Guid, SupportingDoc> repository,
    IMapper mapper,
    ICacheService cacheService,
    ILogger<SupportingDocService> logger) : 
    BaseEntityCacheService<SupportingDoc>(
        cacheService,
        logger,
        CacheKeys.SupportingDoc,
        SupportingDocTags,
        ListTags
    ),
    ISupportingDocService
{
    private static readonly string[] SupportingDocTags = ["supporting-docs", "supporting-doc-list"];
    private static readonly string[] ListTags = ["supporting-doc-list"];

    public async Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));

        try
        {
            var entity = mapper.Map<SupportingDoc>(supportingDocModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = GetCacheKey(createdEntity.Id);
            await SetCacheAsync(cacheKey, createdEntity);

            return createdEntity.Id;
        }
        catch (AutoMapperMappingException ex)
        {
            logger.LogError(ex, "Mapping failed for SupportingDocModel");
            throw new ArgumentException("Invalid data mapping occurred.", ex);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to create SupportingDoc");
            throw new InvalidOperationException("Failed to create SupportingDoc.", ex);
        }
    }

    public async Task DeleteSupportingDocAsync(Guid supportingDocId)
    {
        var cacheKey = GetCacheKey(supportingDocId);
        
        try
        {
            var entity = await repository.GetByIdAsync(supportingDocId)
                ?? throw new KeyNotFoundException($"SupportingDoc with ID {supportingDocId} not found.");

            await repository.DeleteAsync(supportingDocId);
            await ClearCacheAsync(cacheKey);
        }
        catch (KeyNotFoundException ex)
        {
            logger.LogError(ex, "SupportingDoc with ID {SupportingDocId} not found", supportingDocId);
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to delete SupportingDoc ID {SupportingDocId}", supportingDocId);
            throw new InvalidOperationException($"Failed to delete SupportingDoc: {supportingDocId}", ex);
        }
    }

    public async Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id)
    {
        var cacheKey = GetCacheKey(id);
        
        try
        {
            var entity = await cacheService.GetAsync<SupportingDoc>(cacheKey);
            if (entity != null)
            {
                return mapper.Map<SupportingDocModel>(entity);
            }

            entity = await repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException($"SupportingDoc with ID '{id}' not found.");

            await SetCacheAsync(cacheKey, entity);
            
            return mapper.Map<SupportingDocModel>(entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to retrieve SupportingDoc with ID {Id}", id);
            throw new InvalidOperationException($"Failed to retrieve SupportingDoc with ID '{id}'", ex);
        }
    }

    public async Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));
        
        try
        {
            var entity = mapper.Map<SupportingDoc>(supportingDocModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = GetCacheKey(entity.Id);
            await SetCacheAsync(cacheKey, entity);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Failed to update SupportingDoc with ID {Id}", supportingDocModel.Id);
            throw new InvalidOperationException($"Failed to update SupportingDoc with ID '{supportingDocModel.Id}'", ex);
        }
    }
}