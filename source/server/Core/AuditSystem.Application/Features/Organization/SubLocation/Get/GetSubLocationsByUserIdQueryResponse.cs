using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.SubLocation.Get;

public sealed record GetSubLocationsByUserIdQueryResponse(ICollection<SubLocationModel> SubLocations);