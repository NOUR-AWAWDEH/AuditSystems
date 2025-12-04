using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.JobTitle.Delete;

public sealed record DeleteJobTitleCommand(
    Guid Id
) : UserAwareCommand<Result>;