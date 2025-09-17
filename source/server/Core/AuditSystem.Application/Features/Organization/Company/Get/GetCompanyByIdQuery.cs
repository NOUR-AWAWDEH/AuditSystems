using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Organization.Company.Get;

public sealed record class GetCompanyByIdQuery(Guid Id) : IRequest<Result<GetCompanyByIdQueryResponse>>;
