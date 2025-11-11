using Ardalis.Result;
using AuditSystem.Application.Base;
using AuditSystem.Domain.Entities.Common;

namespace AuditSystem.Application.Features.Common.Rating.Create;

public sealed record CreateRateCommand : ICommand<Result<CreateRateCommandResponse>>
{
    public required RateLevel Level { get; set; }
}