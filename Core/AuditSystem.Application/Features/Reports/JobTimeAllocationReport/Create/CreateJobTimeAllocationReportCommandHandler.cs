using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Create;

internal sealed class CreateJobTimeAllocationReportCommandHandler(
    IJobTimeAllocationReportService jobTimeAllocationReportService,
    IMapper mapper,
    ILogger<CreateJobTimeAllocationReportCommandHandler> logger)
    : IRequestHandler<CreateJobTimeAllocationReportCommand, Result<CreateJobTimeAllocationReportCommandResponse>>
{
    public async Task<Result<CreateJobTimeAllocationReportCommandResponse>> Handle(CreateJobTimeAllocationReportCommand request, CancellationToken cancellationToken)
    {
        var jobTimeAllocationReportModel = mapper.Map<JobTimeAllocationReportModel>(request);
        var jobTimeAllocationReportId = await jobTimeAllocationReportService.CreateJobTimeAllocationReportAsync(jobTimeAllocationReportModel);
        logger.LogInformation("Job Time Allocation Report with Name {JobTimeAllocationReportName} has been created.", request.ReportName);

        return Result<CreateJobTimeAllocationReportCommandResponse>.Created(new CreateJobTimeAllocationReportCommandResponse(jobTimeAllocationReportId));
    }
}
