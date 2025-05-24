using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace AuditSystem.Application.Features.SupportingDocs.Create.Upload;

public sealed record class UploadSupportingDocCommand : IRequest<Result<Guid>>
{
    public IFormFile File { get; set; } = null!;
    public Guid UserId { get; set; }
    public string FileName { get; set; } = string.Empty; // User-provided file name
}