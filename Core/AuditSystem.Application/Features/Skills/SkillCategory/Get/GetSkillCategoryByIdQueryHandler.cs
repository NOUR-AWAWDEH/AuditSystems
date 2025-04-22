
using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

namespace AuditSystem.Application.Features.Skills.SkillCategory.Get;

internal sealed class GetSkillCategoryByIdQueryHandler(
    ISkillCategoryService skillCategoryService) : 
    IRequestHandler<GetSkillCategoryByIdQuery, Result<GetSkillCategoryByIdQueryResponse>>
{
    public async Task<Result<GetSkillCategoryByIdQueryResponse>> Handle(GetSkillCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await skillCategoryService.GetSkillCategoryByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetSkillCategoryByIdQueryResponse>.Success(new GetSkillCategoryByIdQueryResponse(result));  
    }
}