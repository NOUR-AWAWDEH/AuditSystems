using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Audit.AuditEngagement.Delete;

public sealed class DeleteAuditEngagementCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
