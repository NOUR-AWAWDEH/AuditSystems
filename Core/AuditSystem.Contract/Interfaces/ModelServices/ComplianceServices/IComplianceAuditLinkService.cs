using AuditSystem.Contract.Models.Compliance;

namespace AuditSystem.Contract.Interfaces.ModelServices.ComplianceServices;

public interface IComplianceAuditLinkService
{
    public Task<Guid> CreateComplianceAuditLinkAsync(ComplianceAuditLinkModel complianceAuditLinkModel);
}
