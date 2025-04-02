using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Models.Checklists;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Checklists.Remark.Create;

internal sealed class CreateRemarkCommandHandler(
    IRemarkService remarkService,
    IMapper mapper,
    ILogger<CreateRemarkCommandHandler> logger)
    : IRequestHandler<CreateRemarkCommand, Result<CreateRemarkCommandResponse>>
{
    public async Task<Result<CreateRemarkCommandResponse>> Handle(CreateRemarkCommand request, CancellationToken cancellationToken)
    {
        var remarkModel = mapper.Map<RemarkModel>(request);
        var remarkId = await remarkService.CreateRemarkAsync(remarkModel);
        logger.LogInformation("Remark with Remark Commants {RemarkCommants} has been created.", request.RemarkCommants);

        return Result<CreateRemarkCommandResponse>.Created(new CreateRemarkCommandResponse(remarkId));
    }
}