using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Checklist.Create;

public sealed record class CreateChecklistCommand : ICommand<Result<CreateChecklistCommandResponse>>
{
    public string Area { get; set; } = string.Empty;
    public string Particulars { get; set; } = string.Empty;
    public string Observation { get; set; } = string.Empty;
}
