using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.RiskAssessment.Get;

public sealed record class GetRiskAssessmentByIdQueryResponse(RiskAssessmentModel RiskAssessmentModel);