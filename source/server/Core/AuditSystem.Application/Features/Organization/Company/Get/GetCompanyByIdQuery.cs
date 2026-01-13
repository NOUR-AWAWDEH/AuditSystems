using Ardalis.Result;
using AuditSystem.Application.Base;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Company.Get;

public sealed record class GetCompanyByIdQuery(Guid Id) : UserAwareQuery<Result<GetCompanyByIdQueryResponse>>;
