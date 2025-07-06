using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Processes.SubProcess.Get;

public sealed record class GetSubProcessByIdQuery(Guid Id) : IRequest<Result<GetSubProcessByIdQueryResponse>>;