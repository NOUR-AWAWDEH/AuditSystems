using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Update;

public sealed record class UpdateAuditProcessCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string ProcessName { get; set; } = string.Empty;
    public required Guid RatingId { get; set; }
    public string Description { get; set; } = string.Empty;
}
