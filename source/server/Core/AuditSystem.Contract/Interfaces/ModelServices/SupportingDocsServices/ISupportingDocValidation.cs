using AuditSystem.Domain.Entities.SupportingDocs;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocValidation
{
    Task<SupportingDoc> ValidateSupportingDocOwnershipAsync(Guid supportingDocId, Guid userId);
}