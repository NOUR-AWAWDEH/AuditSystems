using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Risks.Risk.Get;

public sealed record class GetRiskByIdQuery(Guid Id) : IRequest<Result<GetRiskByIdQueryResponse>>;