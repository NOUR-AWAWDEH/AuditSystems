using AuditSystem.Contract.Interfaces.Cache;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.SupportingDocs;
using AuditSystem.Domain.Entities.SupportingDocs;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Services.EntityServices.SupportingDocsServices;

internal sealed class SupportingDocService(
    IRepository<Guid, SupportingDoc> repository,
    IMapper mapper,
    ICacheService cacheService)
    : ISupportingDocService
{
    public async Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel)
    {
        ArgumentNullException.ThrowIfNull(supportingDocModel, nameof(supportingDocModel));

        try
        {
            var entity = mapper.Map<SupportingDoc>(supportingDocModel);
            var createdEntity = await repository.CreateAsync(entity);
        }
        catch (Exception ex)
        {

        }
        throw new NotImplementedException();
    }
}