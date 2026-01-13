using Ardalis.Result;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using MediatR;

namespace AuditSystem.Application.Features.Organization.Company.Get;

internal sealed class GetCompanyByIdQueryHandler(
    ICompanyService companyService) :
    IRequestHandler<GetCompanyByIdQuery, Result<GetCompanyByIdQueryResponse>>
{
    public async Task<Result<GetCompanyByIdQueryResponse>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await companyService.GetCompanyByIdAsync(request.Id, request.UserId);
        if (result == null)
        {
            return Result.NotFound();
        }

        return Result<GetCompanyByIdQueryResponse>.Success(new GetCompanyByIdQueryResponse(result));
    }
}
