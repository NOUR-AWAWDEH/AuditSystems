using MediatR;
using Ardalis.Result;

namespace AuditSystem.Application.Features.Compliance.ComplianceChecklist.Get;

public sealed record class GetComplianceChecklistByIdQuery(Guid Id) : IRequest<Result<GetComplianceChecklistByIdQueryResponse>>;