using AuditSystem.Contract.Models.Organization;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetById;

public sealed record class GetLocationByIdQueryResponse(LocationModel LocationModel);