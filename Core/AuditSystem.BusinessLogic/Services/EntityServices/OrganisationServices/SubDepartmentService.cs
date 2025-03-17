using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

internal sealed class SubDepartmentService(
    IRepository<Guid, SubDepartment> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISubDepartmentService
{
    public Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        throw new NotImplementedException();
    }
}
