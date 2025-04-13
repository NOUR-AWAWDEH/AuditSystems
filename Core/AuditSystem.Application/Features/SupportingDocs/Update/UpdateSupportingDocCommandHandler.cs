using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Models.SupportingDocs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.SupportingDocs.Update;

internal sealed class UpdateSupportingDocCommandHandler(
    ISupportingDocService supportingDocService,
    IMapper mapper,
    ILogger<UpdateSupportingDocCommandHandler> logger)
    : IRequestHandler<UpdateSupportingDocCommand, Result>
{
    public async Task<Result> Handle(UpdateSupportingDocCommand request, CancellationToken cancellationToken)
    {
        var supportingDocModel = mapper.Map<SupportingDocModel>(request);
        await supportingDocService.UpdateSupportingDocAsync(supportingDocModel);
        
        logger.LogInformation("Supporting document with ID {Id} updated successfully.", request.Id);
        return Result.Success();
    }
}
