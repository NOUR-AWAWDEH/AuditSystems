using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

namespace AuditSystem.Application.Features.Skills.Skill.Get;

internal sealed class GetSkillByIdQueryHandler(
    ISkillService skillService) : 
    IRequestHandler<GetSkillByIdQuery, Result<GetSkillByIdQueryResponse>>
{
    public async Task<Result<GetSkillByIdQueryResponse>> Handle(GetSkillByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await skillService.GetSkillByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetSkillByIdQueryResponse>.Success(new GetSkillByIdQueryResponse(result));
    }
}