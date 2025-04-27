using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Remark.Update;

public sealed record class UpdateRemarkCommand : ICommand<Result>
{
    public required Guid Id { get; init; } = Guid.Empty;
    public required string RemarkCommants { get; set; } = string.Empty;
    public required Guid CheckListCollectionId { get; set; }
}
