using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.RiskControls.RiskControls.Get;

public sealed record class GetRiskControlsByIdQuery(Guid Id) : IRequest<Result<GetRiskControlsByIdQueryResponse>>;