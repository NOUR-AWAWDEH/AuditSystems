using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Domain.Entities.RiskControls;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.RiskControlsServices;

internal sealed class RiskProgramService(
    IRepository<Guid, RiskProgram> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IRiskProgramService
{
    private static readonly string[] RiskProgramTags = ["risk-programs", "risk-program-list"];
    private static readonly string[] ListTags = ["risk-program-list"]; // Tags for collections only

    public async Task<Guid> CreateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));

        try
        {
            var entity = mapper.Map<RiskProgram>(riskProgramModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskProgram, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: RiskProgramTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create RiskProgram.", ex);
        }
    }

    public async Task DeleteRiskProgramAsync(Guid riskProgramId)
    {
        try
        {
            var entity = await repository.GetByIdAsync(riskProgramId);
            if (entity == null)
            {
                throw new KeyNotFoundException($"RiskProgram with ID {riskProgramId} not found.");
            }

            await repository.DeleteAsync(riskProgramId);

            var cacheKey = string.Format(CacheKeys.RiskProgram, riskProgramId);
            await cacheService.RemoveAsync(cacheKey);
            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to delete RiskProgram.", ex);
        }
    }

    public async Task<RiskProgramModel> GetRiskProgramByIdAsync(Guid id)
    {
       try
       {
            var cacheKey = string.Format(CacheKeys.RiskProgram, id);

            var cachedRiskProgram = await cacheService.GetAsync<RiskProgram>(cacheKey);
            if (cachedRiskProgram != null)
            {
                return mapper.Map<RiskProgramModel>(cachedRiskProgram);
            }

            var riskProgram = await repository.GetByIdAsync(id);
            if (riskProgram == null)
            {
                throw new KeyNotFoundException($"RiskProgram with ID {id} not found."); 
            }

            await cacheService.SetAsync(
                key: cacheKey,
                value: riskProgram,
                tags: RiskProgramTags,
                expiration: CacheExpirations.MediumTerm
            ); 
            
            return mapper.Map<RiskProgramModel>(riskProgram);
       }
       catch (Exception ex)
       {
           throw new Exception($"Failed to get RiskProgram by ID {id}.", ex);
       }
    }

    public async Task UpdateRiskProgramAsync(RiskProgramModel riskProgramModel)
    {
        ArgumentNullException.ThrowIfNull(riskProgramModel, nameof(riskProgramModel));

        try
        {
            var entity = mapper.Map<RiskProgram>(riskProgramModel);
            await repository.UpdateAsync(entity);

            var cacheKey = string.Format(CacheKeys.RiskProgram, entity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: entity,
                tags: RiskProgramTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to update RiskProgram.", ex);
        }
    }
}
