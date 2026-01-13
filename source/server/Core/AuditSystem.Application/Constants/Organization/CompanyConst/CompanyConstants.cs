using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.CompanyConst;

public static class CompanyConstants
{
    public static readonly string[] CompanyTags = ["company", "company-list"];
    public static readonly string[] ListTags = ["company-list"];

    public static readonly Expression<Func<Company, Guid>> CompanyOwnerIdSelector =
        d => d.Id; // Note: Company doesn't have CreatedByUserId, validation uses CompanyUser

    public static Expression<Func<Company, bool>> CompanyOwnerPredicate(Guid userId) =>
        c => c.CompanyUsers.Any(cu => cu.AppUserId == userId); // Check if user belongs to company

    public const string EntityName = "Company";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access company {1}.";
    public const string CompanyNotFoundMessage = "Company with ID {0} not found.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedCompanySearchResultsFromCacheMessage = "Retrieved company titles search results from cache";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved user search results from cache";
    public const string UnauthorizedCompanyAccessMessage = "User {0} is not authorized to access company with Id: {1}.";
}
