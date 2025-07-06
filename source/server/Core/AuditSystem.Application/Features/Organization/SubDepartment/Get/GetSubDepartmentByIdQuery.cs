using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Organization.SubDepartment.Get;

public sealed record class GetSubDepartmentByIdQuery(Guid Id) : IRequest<Result<GetSubDepartmentByIdQueryResponse>>;