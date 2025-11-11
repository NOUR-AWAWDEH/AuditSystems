using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Delete;

public sealed record DeleteControlDesignCommand(
    Guid Id
) : UserAwareCommand<Result>;