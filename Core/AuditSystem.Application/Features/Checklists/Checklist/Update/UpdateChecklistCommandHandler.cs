using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Models.Checklists;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Checklists.Checklist.Update;

internal sealed class UpdateChecklistCommandHandler(
    IChecklistService checklistService,
    IMapper mapper,
    ILogger<UpdateChecklistCommandHandler> logger)
    : IRequestHandler<UpdateChecklistCommand, Result>
{
    public async Task<Result> Handle(UpdateChecklistCommand request, CancellationToken cancellationToken)
    {
        var checklistModel = mapper.Map<ChecklistModel>(request);
        await checklistService.UpdateChecklistAsync(checklistModel);

        logger.LogInformation("Checklist updated successfully");
        return Result.Success();
    }
}
