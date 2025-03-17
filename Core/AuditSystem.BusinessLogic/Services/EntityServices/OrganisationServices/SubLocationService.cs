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
    public Task<Guid> CreateSubLocationAsync(SubProcessModel supProcessModel)
    {
        throw new NotImplementedException();
    }
}