using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

namespace AuditSystem.Application.Features.Skills.SkillSet.Get;

internal sealed class GetSkillSetByIdQueryHandler(
    ISkillSetService skillSetService) : 
    IRequestHandler<GetSkillSetByIdQuery, Result<GetSkillSetByIdQueryResponse>>
{
    public async Task<Result<GetSkillSetByIdQueryResponse>> Handle(GetSkillSetByIdQuery request,CancellationToken cancellationToken)
    {
       var result = await skillSetService.GetSkillSetByIdAsync(request.Id);
       if (result == null)
       {
           return Result.NotFound();
       }

       return Result<GetSkillSetByIdQueryResponse>.Success(new GetSkillSetByIdQueryResponse(result));
    }
}