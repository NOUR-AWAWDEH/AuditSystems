using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ProcessesServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Processes;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ProcessesServices;

internal sealed class AuditProcessService(
    IRepository<Guid, AuditProcess> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IAuditProcessService
{
    public async Task<Guid> CreateAuditProcessAsync(AuditProcessModel processModel)
    {
        ArgumentNullException.ThrowIfNull(processModel, nameof(processModel));
        
        try
        {
            var entity = mapper.Map<AuditProcess>(processModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}
