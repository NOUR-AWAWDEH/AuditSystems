using Microsoft.Extensions.Logging;
using MediatR;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

namespace AuditSystem.Application.Features.Organization.Company.Delete;

internal sealed class DeleteCompanyCommandHandler(
    ICompanyService companyService,
    ILogger<DeleteCompanyCommandHandler> logger) : 
    IRequestHandler<DeleteCompanyCommand, Result>
{
    public async Task<Result> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await companyService.DeleteCompanyAsync(request.Id);
            logger.LogInformation("Deleting company with ID {Id}", request.Id);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting company with ID {Id}", request.Id);
            return Result.Error(ex.Message);
        } 
    }
}
