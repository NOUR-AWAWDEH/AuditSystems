using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Remark.Create;

public sealed record class CreateRemarkCommand : ICommand<Result<CreateRemarkCommandResponse>>
{
    public required Guid CheckListManagementId { get; set; }
    public string RemarkCommants { get; set; } = string.Empty;
}