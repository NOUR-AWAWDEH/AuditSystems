
using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get.DownloadByFileId;
public sealed record class DownloadSupportingDocQueryById(Guid Id) 
    : IRequest<Result<DownloadSupportingDocQueryByIdResponse>>;
