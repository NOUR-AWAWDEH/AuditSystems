using AuditSystem.Application.Features.Common.Pagination;
using AuditSystem.Contract.Models.Users;
using FluentValidation;

namespace AuditSystem.Application.Features.Users.Search;

public sealed class SearchUsersQueryValidator : PaginationRequestValidator<UserModel, SearchUsersQuery>
{
    public SearchUsersQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID is required.")
            .Must(BeAValidGuid)
            .WithMessage("User ID must be a valid GUID.");

        RuleFor(x => x.SearchTerm)
            .MaximumLength(200)
            .WithMessage("Search term must not exceed 200 characters.");
    }

    private bool BeAValidGuid(Guid id)
    {
        return id != Guid.Empty;
    }
}
