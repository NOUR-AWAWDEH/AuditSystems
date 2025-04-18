using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices;
using AuditSystem.Contract.Models.Reports;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Reports.JobTimeAllocationReport.Update;

internal sealed class UpdateJobTimeAllocationReportCommandHandler(
    IJobTimeAllocationReportService jobTimeAllocationReportService,
    IMapper mapper,
    ILogger<UpdateJobTimeAllocationReportCommandHandler> logger)
    : IRequestHandler<UpdateJobTimeAllocationReportCommand, Result>
{
    public async Task<Result> Handle(UpdateJobTimeAllocationReportCommand request, CancellationToken cancellationToken)
    {
        var jobTimeAllocationReportModel = mapper.Map<JobTimeAllocationReportModel>(request);
        await jobTimeAllocationReportService.UpdateJobTimeAllocationReportAsync(jobTimeAllocationReportModel);

        logger.LogInformation("Job time allocation report updated successfully");
        return Result.Success();
    }
}