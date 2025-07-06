using FluentValidation;
namespace AuditSystem.Application.Features.Common.Pagination;

public class PaginationRequestValidator<TResponseModel, TPaginationRequest> : AbstractValidator<TPaginationRequest>
where TPaginationRequest : PaginationRequest<TResponseModel>
{
    public PaginationRequestValidator()
    {
        RuleFor(command => command.PageNumber)
            .GreaterThan(0)
            .WithMessage("Page number must be greater than 0.");

        RuleFor(command => command.PageSize)
            .GreaterThan(0)
            .WithMessage("Page sized must be greater than 0.");
    }
}
