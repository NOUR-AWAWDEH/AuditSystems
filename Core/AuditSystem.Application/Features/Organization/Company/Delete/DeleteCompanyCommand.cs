using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Organization.Company.Delete;

public sealed class DeleteCompanyCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
