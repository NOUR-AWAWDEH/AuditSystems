using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Processes.AuditProcess.Get;

public sealed record class GetAuditProcessByIdQuery(Guid Id):IRequest<Result<GetAuditProcessByIdQueryResponse>>;