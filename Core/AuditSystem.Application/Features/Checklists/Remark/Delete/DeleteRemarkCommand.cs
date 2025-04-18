using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.Checklists.Remark.Delete;

public sealed class DeleteRemarkCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}