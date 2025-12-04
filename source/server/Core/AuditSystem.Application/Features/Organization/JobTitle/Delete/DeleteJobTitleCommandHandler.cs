using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.JobTitleServices;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.JobTitle.Delete;

internal sealed class DeleteJobTitleCommandHandler(
    IJobTitleService jobTitleService,
    ILogger<DeleteJobTitleCommandHandler> logger) :
    IRequestHandler<DeleteJobTitleCommand, Result>
{
    public async Task<Result> Handle(DeleteJobTitleCommand request, CancellationToken cancellationToken)
    {
        await jobTitleService.DeleteJobTitleAsync(request.Id, request.UserId);
        logger.LogInformation("Job title with id {JobTitleId} has been deleted by user with id: {UserId}", request.Id, request.UserId);
        return Result.SuccessWithMessage("Job title deleted successfully.");
    }
}