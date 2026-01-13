using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Company.Update;

public sealed record UpdateCompanyCommand(Guid Id, string Name) : UserAwareCommand<Result>;
