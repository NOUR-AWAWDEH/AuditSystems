using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Risks.Risk.Get.GetById;


internal sealed class GetRiskByIdQueryHandler(
    IRiskService riskService,
    ILogger<GetRiskByIdQuery> logger) : 
    IRequestHandler<GetRiskByIdQuery, Result<GetRiskByIdQueryResponse>>
{
    public async Task<Result<GetRiskByIdQueryResponse>> Handle(GetRiskByIdQuery request, CancellationToken cancellationToken)
    {
        var risk = await riskService.GetRiskByIdAsync(request.Id, request.UserId);

        logger.LogInformation("Risk with id: {Id} was retrieved  by user with id: {UserId} successfully", request.Id, request.UserId);
        return Result<GetRiskByIdQueryResponse>.Success(new GetRiskByIdQueryResponse(risk));
    }
}
