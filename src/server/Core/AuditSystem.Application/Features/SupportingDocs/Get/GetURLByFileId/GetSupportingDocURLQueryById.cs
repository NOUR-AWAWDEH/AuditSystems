using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;
public sealed record class GetSupportingDocURLQueryById(Guid Id)
    : IRequest<Result<GetSupportingDocURLQueryByIdResponse>>;
