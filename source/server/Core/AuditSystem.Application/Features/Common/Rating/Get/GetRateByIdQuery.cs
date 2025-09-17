using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Application.Features.Common.Rating.Get;

public sealed record GetRateByIdQuery(Guid Id) : IRequest<Result<GetRateByIdQueryResponse>>;
