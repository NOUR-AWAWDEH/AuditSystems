using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.SupportingDocs.Create;

public sealed record class CreateSupportingDocCommand : ICommand<Result<CreateSupportingDocCommandResponse>>
{
    public required Guid AuditorSettingsId { get; set; }
    public string FileName { get; set; } = string.Empty;
    public int FileSize { get; set; }
    public string URL { get; set; } = string.Empty;
}
