using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Remark.Create;

public sealed record class CreateRemarkCommand : ICommand<Result<CreateRemarkCommandResponse>>
{
    public required string RemarkCommants { get; set; } = string.Empty;
    public required Guid CheckListCollectionId { get; set; }
}