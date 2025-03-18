using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Processes;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

internal sealed class SubLocationService(
    IRepository<Guid, SubLocation> repository,
    IMapper mapper,
    ICacheService cacheService) 
    : ISubLocationService
{
    public async Task<Guid> CreateSubLocationAsync(SubProcessModel supProcessModel)
    {
        ArgumentNullException.ThrowIfNull(supProcessModel, nameof(supProcessModel));
        
        try
        {
            var entity = mapper.Map<SubLocation>(supProcessModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}