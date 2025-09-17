using Ardalis.Result;
using AuditSystem.Application.Base;
using AuditSystem.Domain.Entities.Common;


namespace AuditSystem.Application.Features.Common.Rating.Update;

public sealed record UpdateRateCommand : ICommand<Result> 
{ 
    public required Guid Id { get; init; } = Guid.Empty;

    public required RateLevel Level { get; init; }

}
