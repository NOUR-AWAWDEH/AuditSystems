using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get;

public sealed record class GetRiskControlMatrixByIdQuery(Guid Id) : IRequest<Result<GetRiskControlMatrixByIdQueryResponse>>;