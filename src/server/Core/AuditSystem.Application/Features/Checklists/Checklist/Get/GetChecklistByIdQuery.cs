using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Checklists.Checklist.Get;

public sealed record class GetChecklistByIdQuery(Guid Id) : IRequest<Result<GetChecklistByIdQueryResponse>>;