using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Tasks.Get;

public sealed record class GetTaskManagementByIdQuery(Guid Id) : IRequest<Result<GetTaskManagementByIdQueryResponse>>;