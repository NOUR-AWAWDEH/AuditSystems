using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetByUserId;

public sealed record GetLocationsByUserIdQueryResponse(IReadOnlyList<LocationModel> Locations);