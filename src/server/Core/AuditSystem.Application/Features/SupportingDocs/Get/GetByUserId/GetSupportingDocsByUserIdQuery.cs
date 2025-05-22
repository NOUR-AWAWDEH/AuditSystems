using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetByUserId;

public sealed record class GetSupportingDocsByUserIdQuery(Guid Id) : IRequest<Result<GetSupportingDocsByUserIdQueryResponse>>;