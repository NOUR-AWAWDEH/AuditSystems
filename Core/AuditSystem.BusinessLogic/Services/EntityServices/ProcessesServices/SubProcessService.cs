using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class SubProcessService(
    IRepository<Guid, SubProcess> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISubProcessService
{
    private static readonly string[] SubProcessTags = ["sub-processes", "sub-process-list"];
    private static readonly string[] ListTags = ["sub-process-list"]; // Tags for collections only

    public async Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));
        
        try
        {
            var entity = mapper.Map<SubProcess>(subProcessModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SubProcess, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SubProcessTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SubProcess.", ex);
        }
    }
}
