using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.RiskControlMatrixServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.RiskControls.RiskControlMatrix.Get.GetById;


internal sealed class GetRiskControlMatrixByIdQueryHandler(
    IRiskControlMatrixService riskControlMatrixService,
    ILogger<GetRiskControlMatrixByIdQueryHandler> logger) : 
    IRequestHandler<GetRiskControlMatrixByIdQuery, Result<GetRiskControlMatrixByIdQueryResponse>>
{
    public async Task<Result<GetRiskControlMatrixByIdQueryResponse>> Handle(GetRiskControlMatrixByIdQuery request, CancellationToken cancellationToken)
    {
        var riskControlMatrix = await riskControlMatrixService.GetRiskControlMatrixByIdAsync(request.Id, request.UserId);

        logger.LogInformation("Risk control matrix with id: {Id} was retrieved by UserId: {UserId}", request.Id, request.UserId);
        return Result<GetRiskControlMatrixByIdQueryResponse>.Success(new GetRiskControlMatrixByIdQueryResponse(riskControlMatrix));
    }
}
