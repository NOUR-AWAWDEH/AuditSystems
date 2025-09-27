using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Update;

public sealed record class UpdateControlDesignCommand(
    Guid Id,
    string Description,
    Guid ExitControlId,
    bool Inadequate
) : UserAwareCommand<Result>;
