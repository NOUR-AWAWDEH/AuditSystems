using AuditSystem.Contract.Models.Risks;

namespace AuditSystem.Application.Features.Risks.RiskFactor.Get;

public sealed record class GetRiskFactorByIdQueryResponse(RiskFactorModel RiskFactorModel);