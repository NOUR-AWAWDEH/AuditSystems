using Microsoft.Extensions.Logging;
using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.SkillsServices;

namespace AuditSystem.Application.Features.Skills.Skill.Delete;

internal sealed class DeleteSkillCommandHandler(
    ISkillService skillService,
    ILogger<DeleteSkillCommandHandler> logger) :
    IRequestHandler<DeleteSkillCommand, Result>
{
    public async Task<Result> Handle(DeleteSkillCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await skillService.DeleteSkillAsync(request.Id);
            logger.LogInformation("Skill with id {id} was deleted", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogInformation("An error occurred while deleting skill with id {id}: {error}", request.Id, ex.Message);
            return Result.Error(ex.Message);
        }
    }
}