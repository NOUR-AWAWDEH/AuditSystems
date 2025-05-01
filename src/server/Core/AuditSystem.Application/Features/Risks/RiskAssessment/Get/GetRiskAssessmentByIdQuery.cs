using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Get;

public sealed record class GetRiskAssessmentByIdQuery(Guid Id) : IRequest<Result<GetRiskAssessmentByIdQueryResponse>>;