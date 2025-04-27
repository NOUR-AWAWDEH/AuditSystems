using AuditSystem.Contract.Models.Audit;

namespace AuditSystem.Application.Features.Audit.BusinessObjective.Get;

public sealed record class GetBusinessObjectiveByIdQueryResponse(BusinessObjectiveModel BusinessObjectiveModel);