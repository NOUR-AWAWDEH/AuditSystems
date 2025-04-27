using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Organization.Department.Get;

public sealed record class GetDepartmentByIdQuery(Guid Id) : IRequest<Result<GetDepartmentByIdQueryResponse>>;