using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Models.Checklists;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Checklists.Checklist.Create;

internal sealed class CreateChecklistCommandHandler(
    IChecklistService checklistService,
    IMapper mapper,
    ILogger<CreateChecklistCommandHandler> logger)
    : IRequestHandler<CreateChecklistCommand, Result<CreateChecklistCommandResponse>>
{
    public async Task<Result<CreateChecklistCommandResponse>> Handle(CreateChecklistCommand request, CancellationToken cancellationToken)
    {
        var checklistModel = mapper.Map<ChecklistModel>(request);
        var checklistId = await checklistService.CreateCheckListAsync(checklistModel);
        logger.LogInformation("Checklist with Area {ChecklistArea} has been created.", request.Area);

        return Result<CreateChecklistCommandResponse>.Created(new(checklistId));
    }
}