using AuditSystem.Contract.Models.Observation;

namespace AuditSystem.Application.Features.Observation.Get.GetByUserId;

public sealed record GetObservationsByUserIdQueryResponse(IReadOnlyList<ObservationModel> Observations);
