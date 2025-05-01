using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
namespace AuditSystem.Application.Features.Checklists.Remark.Get;

internal sealed class GetRemarkByIdQueryHandler(
    IRemarkService remarkService) :
    IRequestHandler<GetRemarkByIdQuery, Result<GetRemarkByIdQueryResponse>>
{
    public async Task<Result<GetRemarkByIdQueryResponse>> Handle(GetRemarkByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await remarkService.GetRemarkByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRemarkByIdQueryResponse>.Success(new GetRemarkByIdQueryResponse(result));
    }
}
