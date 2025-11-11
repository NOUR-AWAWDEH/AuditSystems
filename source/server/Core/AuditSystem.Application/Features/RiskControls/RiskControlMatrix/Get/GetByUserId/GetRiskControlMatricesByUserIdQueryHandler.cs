using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get.GetByUserId;

internal sealed class GetRiskControlMatricesByUserIdQueryHandler(
    IRiskControlMatrixService riskControlMatrixService,
    ILogger<GetRiskControlMatricesByUserIdQueryHandler> logger) : 

    IRequestHandler<GetRiskControlMatricesByUserIdQuery, Result<GetRiskControlMatricesByUserIdQueryResponse>>
{
    public async Task<Result<GetRiskControlMatricesByUserIdQueryResponse>> Handle(
        GetRiskControlMatricesByUserIdQuery request,
        CancellationToken cancellationToken)
    {
        var riskControlMatrices = await riskControlMatrixService.GetRiskControlMatricesByUserIdAsync(request.UserId);

        logger.LogInformation("Risk control matrices with user id: {UserId} was retrieved", request.UserId);
        return Result<GetRiskControlMatricesByUserIdQueryResponse>.Success(new GetRiskControlMatricesByUserIdQueryResponse(riskControlMatrices.ToList()), "Risk Control Matrices retrieved successfully");
    }
}