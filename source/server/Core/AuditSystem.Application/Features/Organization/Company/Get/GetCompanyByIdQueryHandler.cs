using Ardalis.Result;
using MediatR;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;

namespace AuditSystem.Application.Features.Organization.Company.Get;

internal sealed class GetCompanyByIdQueryHandler(
    ICompanyService companyService) : 
    IRequestHandler<GetCompanyByIdQuery, Result<GetCompanyByIdQueryResponse>>
{
    public async Task<Result<GetCompanyByIdQueryResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await companyService.GetCompanyByIdAsync(request.Id);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetCompanyByIdQueryResponse>.Success(new GetCompanyByIdQueryResponse(result));
    }
}
