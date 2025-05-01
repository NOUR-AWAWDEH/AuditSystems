using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.SupportingDocs.Get;

public sealed record class GetSupportingDocsByIdQuery(Guid Id) : IRequest<Result<GetSupportingDocsByIdQueryResponse>>;