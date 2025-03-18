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
    public async Task<Guid> CreateSubProcessAsync(SubProcessModel subProcessModel)
    {
        ArgumentNullException.ThrowIfNull(subProcessModel, nameof(subProcessModel));
        
        try
        {
            var entity = mapper.Map<SubProcess>(subProcessModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
