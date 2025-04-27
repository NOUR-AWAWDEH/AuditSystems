using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.SupportingDocs.Update;

public sealed record class UpdateSupportingDocCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string FileName { get; set; } = string.Empty;
    public required int FileSize { get; set; }
    public required string URL { get; set; } = string.Empty;
}