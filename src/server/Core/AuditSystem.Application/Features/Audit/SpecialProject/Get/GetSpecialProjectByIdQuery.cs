using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Get;

public sealed record class GetSpecialProjectByIdQuery(Guid Id) : IRequest<Result<GetSpecialProjectByIdQueryResponse>>;