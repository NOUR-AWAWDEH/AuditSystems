using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Common.Rating.Update;

public sealed record class UpdateRatingCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string Level { get; init; } = string.Empty;
}
