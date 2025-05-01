using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Get;

public sealed record class GetRiskProgramByIdQuery(Guid Id) : IRequest<Result<GetRiskProgramByIdQueryResponse>>;