using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.ChecklistServices;
using AuditSystem.Contract.Models.Checklists;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Checklists.Remark.Update;

internal sealed class UpdateRemarkCommandHandler(
    IRemarkService remarkService,
    IMapper mapper,
    ILogger<UpdateRemarkCommandHandler> logger)
    : IRequestHandler<UpdateRemarkCommand, Result>
{
    public async Task<Result> Handle(UpdateRemarkCommand request, CancellationToken cancellationToken)
    {
        var remarkModel = mapper.Map<RemarkModel>(request);
        await remarkService.UpdateRemarkAsync(remarkModel);

        logger.LogInformation("Remark updated successfully");
        return Result.Success();
    }
}
