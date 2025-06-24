using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Location.Get.GetByUserId;

public sealed record class GetLocationsByUserIdQuery(Guid UserId) : IRequest<Result<GetLocationsByUserIdQueryResponse>>;