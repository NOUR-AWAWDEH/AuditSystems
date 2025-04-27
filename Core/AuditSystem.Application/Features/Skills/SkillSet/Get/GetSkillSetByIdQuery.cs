using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Skills.SkillSet.Get;

public sealed record class GetSkillSetByIdQuery(Guid Id): IRequest<Result<GetSkillSetByIdQueryResponse>>;