using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices;
using Microsoft.Identity.Client;

namespace AuditSystem.Application.Features.RiskControls.RiskProgram.Get;

internal sealed class GetRiskProgramByIdQueryHandler(IRiskProgramService riskProgramService) :
    IRequestHandler<GetRiskProgramByIdQuery, Result<GetRiskProgramByIdQueryResponse>>
{
    public async Task<Result<GetRiskProgramByIdQueryResponse>> Handle(GetRiskProgramByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await riskProgramService.GetRiskProgramByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetRiskProgramByIdQueryResponse>.Success(new GetRiskProgramByIdQueryResponse(result));
    }
}
