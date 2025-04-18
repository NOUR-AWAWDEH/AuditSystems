using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Common.Rating.Delete;

public sealed class DeleteRatingCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}
