using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Get;

internal sealed class GetSpecialProjectByIdQueryHandler(
    ISpecialProjectService specialProjectService) : 
    IRequestHandler<GetSpecialProjectByIdQuery, Result<GetSpecialProjectByIdQueryResponse>>
{
    public async Task<Result<GetSpecialProjectByIdQueryResponse>> Handle(GetSpecialProjectByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await specialProjectService.GetSpecialProjectByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetSpecialProjectByIdQueryResponse>.Success(new GetSpecialProjectByIdQueryResponse(result)); 
    }
}
