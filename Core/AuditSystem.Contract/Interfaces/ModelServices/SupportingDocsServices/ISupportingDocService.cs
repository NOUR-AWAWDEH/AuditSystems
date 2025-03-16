using AuditSystem.Contract.Models.SupportingDocs;

namespace AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;

public interface ISupportingDocService
{
    public Task<Guid> CreateSupportingDocAsync(SupportingDocModel supportingDocModel);
}
