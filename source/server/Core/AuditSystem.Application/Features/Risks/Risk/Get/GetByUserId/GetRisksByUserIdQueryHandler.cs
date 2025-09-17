using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Get.GetByUserId;


internal sealed class GetRisksByUserIdQueryHandler(
    IRiskService riskService,
    ILogger<GetRisksByUserIdQuery> logger) : 
    IRequestHandler<GetRisksByUserIdQuery, Result<GetRisksByUserIdQueryResponse>>
{
    public async Task<Result<GetRisksByUserIdQueryResponse>> Handle(
        GetRisksByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var risks = await riskService.GetRisksByUserIdAsync(request.UserId);

        logger.LogInformation("Risks were retrieved  by user with id: {UserId} successfully", request.UserId);
        return Result<GetRisksByUserIdQueryResponse>.Success(new GetRisksByUserIdQueryResponse(risks));
    }
}
