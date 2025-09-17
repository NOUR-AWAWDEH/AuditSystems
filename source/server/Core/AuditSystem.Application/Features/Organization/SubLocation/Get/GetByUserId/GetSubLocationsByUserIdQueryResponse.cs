using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get.GetByUserId;

public sealed record GetSubLocationsByUserIdQueryResponse(IReadOnlyList<SubLocationModel> SubLocations);