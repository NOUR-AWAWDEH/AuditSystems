using Ardalis.Result;
using AuditSystem.Application.Base;

namespace AuditSystem.Application.Features.SupportingDocs.Delete;

public sealed record DeleteSupportingDocCommand(
    Guid Id
) : UserAwareCommand<Result>;