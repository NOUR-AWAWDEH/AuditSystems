using Ardalis.Result;
using AuditSystem.Application.Base;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get.GetURLByFileId;
public sealed record GetSupportingDocURLQueryById(
    Guid Id
) : UserAwareQuery<Result<GetSupportingDocURLQueryByIdResponse>>;
