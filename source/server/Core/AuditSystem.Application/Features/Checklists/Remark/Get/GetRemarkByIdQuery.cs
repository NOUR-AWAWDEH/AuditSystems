using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Checklists.Remark.Get;

public sealed record class GetRemarkByIdQuery(Guid Id) : IRequest<Result<GetRemarkByIdQueryResponse>>;