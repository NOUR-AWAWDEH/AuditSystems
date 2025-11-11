using AuditSystem.Contract.Models.Observation;

namespace AuditSystem.Application.Features.Observation.Get.GetById;

public sealed record class GetObservationByIdQueryResponse(ObservationModel ObservationModel);
