using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Risks.ControlDesign.Create;

public sealed record class CreateControlDesignCommand(
    string Description,
    Guid ExitControlId
) : UserAwareCommand<Result<CreateControlDesignCommandResponse>>;
