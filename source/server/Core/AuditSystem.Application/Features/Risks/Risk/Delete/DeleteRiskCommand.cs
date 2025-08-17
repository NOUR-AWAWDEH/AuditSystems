using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.Risk.Delete;

public sealed record DeleteRiskCommand(
    Guid Id
) : UserAwareCommand<Result>;