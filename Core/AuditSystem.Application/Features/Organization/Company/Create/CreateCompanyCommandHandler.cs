using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Models.Organization;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace AuditSystem.Application.Features.Organization.Company.Create;

internal sealed class CreateCompanyCommandHandler(
    ICompanyService companyService,
    IMapper mapper,
    ILogger<CreateCompanyCommandHandler> logger)
    : IRequestHandler<CreateCompanyCommand, Result<CreateCompanyCommandResponse>>
{
    public async Task<Result<CreateCompanyCommandResponse>> Handle(CreateCompanyCommand request, CancellationToken cancellationToken)
    {
        var companyModel = mapper.Map<CompanyModel>(request);
        var companyId = await companyService.CreateCompanyAsync(companyModel);
        logger.LogInformation("Company with Name {CompanyName} has been created.", request.Name);

        return Result<CreateCompanyCommandResponse>.Created(new CreateCompanyCommandResponse(companyId));
    }
}
