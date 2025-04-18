using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Update;

public sealed record class UpdateComplianceChecklistCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Area { get; set; } = string.Empty;
    public required string Subject { get; set; } = string.Empty;
    public required string Particulars { get; set; } = string.Empty;
}
