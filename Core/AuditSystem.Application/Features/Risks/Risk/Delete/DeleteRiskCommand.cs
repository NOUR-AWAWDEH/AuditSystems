using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Delete;

public sealed class DeleteRiskCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}