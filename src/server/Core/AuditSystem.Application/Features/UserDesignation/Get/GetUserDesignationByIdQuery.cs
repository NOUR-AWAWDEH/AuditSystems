using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.UserDesignation.Get;

public sealed record class GetUserDesignationByIdQuery(Guid Id) : IRequest<Result<GetUserDesignationByIdQueryResponse>>;