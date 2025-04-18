﻿using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;

internal sealed class SupportingDocService(
    IRepository<Guid, SupportingDoc> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISupportingDocService
{
    private static readonly string[] SupportingDocTags = ["supporting-docs", "supporting-doc-list"];
    private static readonly string[] ListTags = ["supporting-doc-list"]; // Tags for collections only

    public async Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));

        try
        {
            var entity = mapper.Map<SupportingDoc>(supportingDocModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SupportingDoc, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SupportingDocTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SupportingDoc.", ex);
        }
    }

    public async Task DeleteSupportingDocAsync(Guid supportingDocId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(supportingDocId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"SupportingDoc with ID {supportingDocId} not found.");
            }

            await repository.DeleteAsync(supportingDocId);

            var cacheKey = string.Format(CacheKeys.SupportingDoc, supportingDocId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete SupportingDoc.", ex);
        }
    }

    public async Task<SupportingDocModel> GetSupportingDocByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.SupportingDoc, id);

            var cachedEntity = await cacheService.GetAsync<SupportingDoc>(cacheKey);
            if (cachedEntity != null)
            {
                return mapper.Map<SupportingDocModel>(cachedEntity);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"SupportingDoc with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SupportingDocTags,
                expiration: CacheExpirations.MediumTerm
            ); 
            
            return mapper.Map<SupportingDocModel>(entity);
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get SupportingDoc ID {id}.", ex);
        }
    }

    public async Task UpdateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));

        try
        {
            var entity = mapper.Map<SupportingDoc>(supportingDocModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SupportingDoc, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: SupportingDocTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update SupportingDoc.", ex);
        }
    }
}