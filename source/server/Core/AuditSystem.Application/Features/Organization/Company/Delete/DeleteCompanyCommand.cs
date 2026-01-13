using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Company.Delete;

public sealed record DeleteCompanyCommand(Guid Id) : UserAwareCommand<Result>;
