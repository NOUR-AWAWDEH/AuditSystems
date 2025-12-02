using System.Linq.Expressions;
using AuditSystem.Domain.Entities.Organization;

namespace AuditSystem.Application.Constants.Organization.JobTitleConst;

public static class JobTitleConstants
{
    public static readonly string[] JobTitleTags = ["jobtitle", "jobtitle-list"];
    public static readonly string[] ListTags = ["jobtitle-list"];

    public static readonly Expression<Func<JobTitle, Guid>> JobTitleCompanyIdSelector =
        d => d.CompanyId;
    public const string EntityName = "JobTitle";

    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access job title {1}.";
    public const string JobTitleNotFoundMessage = "Job title with ID {0} not found.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedCompanySearchResultsFromCacheMessage = "Retrieved company job titles search results from cache";
}
