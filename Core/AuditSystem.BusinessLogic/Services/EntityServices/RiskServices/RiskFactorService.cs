﻿using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskServices;

internal sealed class RiskFactorService(
    IRepository<Guid, RiskFactor> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskFactorService
{
    private static readonly string[] RiskFactorTags = ["risk-factors", "risk-factor-list"];
    private static readonly string[] ListTags = ["risk-factor-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));

        try
        {
            var entity = mapper.Map<RiskFactor>(riskFactorModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskFactor, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RiskFactorTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create RiskFactor.", ex);
        }
    }

    public async Task DeleteRiskFactorAsync(Guid riskFactorId)
    {
        try 
        {
            var entity = await repository.GetByIdAsync(riskFactorId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskFactor with ID {riskFactorId} not found.");
            }

            await repository.DeleteAsync(riskFactorId);
            
            var cacheKey = string.Format(CacheKeys.RiskFactor, riskFactorId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete RiskFactor.", ex);
        }
    }

    public async Task<RiskFactorModel> GetRiskFactorByIdAsync(Guid id)
    {
        try
        {
            var cacheKey = string.Format(CacheKeys.RiskFactor, id);

            var cachedRiskFactor = await cacheService.GetAsync<RiskFactor>(cacheKey);
            if (cachedRiskFactor != null)
            {
                return mapper.Map<RiskFactorModel>(cachedRiskFactor);
            }

            var entity = await repository.GetByIdAsync(id);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskFactor with ID {id} not found.");
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskFactorTags,
                expiration: CacheExpirations.MediumTerm
            );

            return mapper.Map<RiskFactorModel>(entity); 
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to get RiskFactor by ID {id}.", ex);
        }
    }

    public async Task UpdateRiskFactorAsync(RiskFactorModel riskFactorModel)
    {
        ArgumentNullException.ThrowIfNull(riskFactorModel, nameof(riskFactorModel));

        try
        {
            var entity = mapper.Map<RiskFactor>(riskFactorModel);
            await repository.UpdateAsync(entity);
            
            var cacheKey = string.Format(CacheKeys.RiskFactor, entity.Id);
            
            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskFactorTags,
                expiration: CacheExpirations.MediumTerm);
            
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update RiskFactor.", ex);
        }
    }
}