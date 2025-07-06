using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Create;

public sealed record class CreateComplianceChecklistCommand : ICommand<Result<CreateComplianceChecklistCommandResponse>>
{
    public required string Area { get; set; } = string.Empty;
    public required string Subject { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
}
