using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Delete;

public sealed class DeleteRiskControlsCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}