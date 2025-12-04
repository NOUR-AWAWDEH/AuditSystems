using Ardalis.Result;
using AuditSystem.Application.Base;
namespace AuditSystem.Application.Features.Organization.JobTitle.Update;

public sealed record UpdateJobTitleCommand(
    Guid Id,
    string Name,
    Guid CompanyId
) : UserAwareCommand<Result>;