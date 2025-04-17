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

    public Task<RiskProgramModel> GetRiskProgramByIdAsync(Guid Id)
    {
        throw new NotImplementedException();
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
