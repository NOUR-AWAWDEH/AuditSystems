using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Common.Rating.Get;

public sealed record class GetRatingByIdQuery(Guid Id) : IRequest<Result<GetRatingByIdQueryResponse>>; 