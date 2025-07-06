using Ardalis.Result;
using AuditSystem.Application.Base;
using Microsoft.AspNetCore.Http;

namespace AuditSystem.Application.Features.SupportingDocs.Create.Upload;

public sealed record UploadSupportingDocCommand(
    IFormFile File,
    string FileName
) : UserAwareCommand<Result<UploadSupportingDocCommandResponse>>;