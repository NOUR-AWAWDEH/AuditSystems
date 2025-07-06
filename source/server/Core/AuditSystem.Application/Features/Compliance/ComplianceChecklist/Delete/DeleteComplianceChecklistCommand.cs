using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Delete;

public sealed class DeleteComplianceChecklistCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
