using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.OrganisationServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.OrganisationServices;

internal sealed class DepartmentService(
    IRepository<Guid, Department> repository,
    IMapper mapper,
    ICacheService cacheService)
    : IDepartmentService
{
    public Task<Guid> CreateDepartmentAsync(DepartmentModel departmentModel)
    {
        throw new NotImplementedException();
    }
}