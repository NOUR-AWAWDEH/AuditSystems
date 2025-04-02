using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;

public sealed record class CreateComplianceChecklistCommand : ICommand<Result<CreateComplianceChecklistCommandResponse>>
{
    public string Area { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
}
