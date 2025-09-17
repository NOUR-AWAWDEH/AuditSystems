using System.Linq.Expressions;
using AuditSystem.Domain.Entities.SupportingDocs;
using SupportingDocEntity = AuditSystem.Domain.Entities.SupportingDocs.SupportingDoc;

namespace AuditSystem.Application.Constants.SupportingDocs;

public static class SupportingDocsConstants
{
    public static readonly string[] SupportingDocTags = ["supporting-docs", "supporting-doc-list"];
    public static readonly string[] ListTags = ["supporting-doc-list"];
    public static readonly Expression<Func<SupportingDocEntity, object?>>[] IncludeUser = [s => s.User];
    
    public const string EntityName = "SupportingDoc";
    public const string ServiceName = "SupportingDocs";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access supporting document {1}.";
    public const string SupportingDocNotFoundMessage = "Supporting document with ID {0} not found.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string SupportingDocIdEmptyMessage = "SupportingDocId cannot be empty";
    public const string CreationFailedMessage = "Failed to create supporting document.";
}