using Ardalis.Result;
using MediatR;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Get;

public sealed record class GetSkillCategoryByIdQuery(Guid Id): IRequest<Result<GetSkillCategoryByIdQueryResponse>>;
