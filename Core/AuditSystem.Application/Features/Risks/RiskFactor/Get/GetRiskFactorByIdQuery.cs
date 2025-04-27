using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Get;

public sealed record class GetRiskFactorByIdQuery(Guid Id) : IRequest<Result<GetRiskFactorByIdQueryResponse>>;
