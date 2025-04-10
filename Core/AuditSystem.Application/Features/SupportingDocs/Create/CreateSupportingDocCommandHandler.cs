using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using AuditSystem.Contract.Models.SupportingDocs;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.SupportingDocs.Create;

internal class CreateSupportingDocCommandHandler(
    ISupportingDocService supportingDocService,
    IMapper mapper,
    ILogger<CreateSupportingDocCommandHandler> logger)
    : IRequestHandler<CreateSupportingDocCommand, Result<CreateSupportingDocCommandResponse>>
{
    public async Task<Result<CreateSupportingDocCommandResponse>> Handle(CreateSupportingDocCommand request, CancellationToken cancellationToken)
    {
        var supportingDocModel = mapper.Map<SupportingDocModel>(request);
        var supportingDocId = await supportingDocService.CreateSupportingDocAsync(supportingDocModel);
        logger.LogInformation("Supporting Doc with File Name {SupportingDocFileName} has been created.", request.FileName);

        return Result<CreateSupportingDocCommandResponse>.Created(new CreateSupportingDocCommandResponse(supportingDocId));
    }
}
