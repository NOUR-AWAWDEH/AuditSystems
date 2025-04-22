using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.Objective.Get;

public sealed record class GetObjectiveByIdQueryResponse(ObjectiveModel ObjectiveModel);