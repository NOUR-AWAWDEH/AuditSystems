using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Skills.Skill.Get;

public sealed record class GetSkillByIdQuery(Guid Id) : IRequest<Result<GetSkillByIdQueryResponse>>;