using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.JobTitle.Create;

public sealed record CreateJobTitleCommand(
    string Name,
    Guid CompanyId
) : UserAwareCommand<Result<CreateJobTitleCommandResponse>>;