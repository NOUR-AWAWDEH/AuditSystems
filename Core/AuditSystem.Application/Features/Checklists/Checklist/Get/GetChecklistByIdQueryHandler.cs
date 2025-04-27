using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
namespace AuditSystem.Application.Features.Checklists.Checklist.Get;

internal sealed class GetChecklistByIdQueryHandler(
    IChecklistService checklistService) : 
    IRequestHandler<GetChecklistByIdQuery, Result<GetChecklistByIdQueryResponse>>
{
    public async Task<Result<GetChecklistByIdQueryResponse>> Handle(GetChecklistByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await checklistService.GetChecklistByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();   
        }

        return Result<GetChecklistByIdQueryResponse>.Success(new GetChecklistByIdQueryResponse(result));
    }
}
