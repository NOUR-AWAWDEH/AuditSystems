using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Company.Update;

internal sealed class UpdateCompanyCommandHandler(
    ICompanyService companyService,
    IMapper mapper,
    ILogger<UpdateCompanyCommandHandler> logger)
    : IRequestHandler<UpdateCompanyCommand, Result>
{
    public async Task<Result> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyModel = mapper.Map<CompanyModel>(request);
        await companyService.UpdateCompanyAsync(companyModel);
        
        logger.LogInformation("Company updated successfully");
        return Result.Success();
    }
}
