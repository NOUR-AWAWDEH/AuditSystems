using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.UserDesignationServices;
namespace AuditSystem.Application.Features.UserDesignation.Get;

internal sealed class GetUserDesignationByIdQueryHandler(
    IUserDesignationService userDesignationService) : 
    IRequestHandler<GetUserDesignationByIdQuery, Result<GetUserDesignationByIdQueryResponse>>
{
    public async Task<Result<GetUserDesignationByIdQueryResponse>> Handle(GetUserDesignationByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await userDesignationService.GetUserDesignationByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetUserDesignationByIdQueryResponse>.Success(new GetUserDesignationByIdQueryResponse(result));
    }
}