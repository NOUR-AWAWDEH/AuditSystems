using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Compliance;
using AuditSystem.Domain.Entities.Compliance;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.ComplianceServices;

internal sealed class ComplianceChecklistService(
    IRepository<Guid, ComplianceChecklist> repository,
    IMapper mapper,
    ICacheService cacheService
    )
    : IComplianceChecklistService
{
    private static readonly string[] ComplianceChecklistTags = ["compliance-checklists", "compliance-checklist-list"];
    private static readonly string[] ListTags = ["compliance-checklist-list"]; // Tags for collections only

    public async Task<Guid> CreateComplianceChecklistAsync(ComplianceChecklistModel complianceChecklistModel)
    {
        ArgumentNullException.ThrowIfNull(complianceChecklistModel, nameof(complianceChecklistModel));

        try
        {
            var entity = mapper.Map<ComplianceChecklist>(complianceChecklistModel);
            var createdEntity = await repository.CreateAsync(entity);

            var cacheKey = string.Format(CacheKeys.ComplianceChecklist, createdEntity.Id);

            await cacheService.SetAsync(
                key: cacheKey,
                value: createdEntity,
                tags: ComplianceChecklistTags,
                expiration: CacheExpirations.MediumTerm);

            await cacheService.RemoveCacheByTagAsync(ListTags);

            return createdEntity.Id;
        }
        catch (Exception ex)
        {
            throw new Exception("Failed to create ComplianceChecklist.", ex);
        }
    }
}
