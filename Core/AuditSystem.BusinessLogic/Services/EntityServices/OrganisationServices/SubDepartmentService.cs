using AuditSystem.Application.Constants;
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
    private static readonly string[] SubDepartmentTags = ["sub-departments", "sub-department-list"];
    private static readonly string[] ListTags = ["sub-department-list"]; // Tags for collections only

    public async Task<Guid> CreateSubDepartmentAsync(SubDepartmentModel subDepartmentModel)
    {
        ArgumentNullException.ThrowIfNull(subDepartmentModel, nameof(subDepartmentModel));
        
        try
        {
            var entity = mapper.Map<SubDepartment>(subDepartmentModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.SubDepartment, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: SubDepartmentTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create SubDepartment.", ex);
        }
    }
}
