using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Common.Rating.Create;

public sealed record CreateRatingCommand : ICommand<Result<CreateRatingCommandResponse>>
{
    public required string Level { get; init; } = string.Empty;
}
