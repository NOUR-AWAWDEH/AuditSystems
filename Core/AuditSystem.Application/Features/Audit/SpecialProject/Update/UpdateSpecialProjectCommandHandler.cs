using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices;
using AuditSystem.Contract.Models.Audit;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Audit.SpecialProject.Update;

internal sealed class UpdateSpecialProjectCommandHandler(
    ISpecialProjectService specialProjectService,
    IMapper mapper,
    ILogger<UpdateSpecialProjectCommandHandler> logger)
    : IRequestHandler<UpdateSpecialProjectCommand, Result>
{
    public async Task<Result> Handle(UpdateSpecialProjectCommand request, CancellationToken cancellationToken)
    {
        var specialProjectModel = mapper.Map<SpecialProjectModel>(request);
        await specialProjectService.UpdateSpecialProjectAsync(specialProjectModel);

        logger.LogInformation("Special project updated successfully");
        return Result.Success();
    }
}
