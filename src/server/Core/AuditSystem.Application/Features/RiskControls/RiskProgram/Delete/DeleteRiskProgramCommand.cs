using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Delete;

public sealed class DeleteRiskProgramCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}