using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.SupportingDocs.Delete;

public sealed class DeleteSupportingDocCommand : ICommand<Result>
{
    public required Guid Id { get; set; } = Guid.Empty;
}