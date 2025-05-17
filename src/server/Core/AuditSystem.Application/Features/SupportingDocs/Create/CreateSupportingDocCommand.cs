using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.SupportingDocs.Create;

public sealed record class CreateSupportingDocCommand : ICommand<Result<CreateSupportingDocCommandResponse>>
{
    public required string FileName { get; set; } = string.Empty;
    public required long FileSize { get; set; }
    public Guid DriveFileId { get; set; }
    public required string URL { get; set; } = string.Empty;
}
