namespace AuditSystem.Application.Constants.AppUserConst;

public static class AppUserConstants
{
    public static readonly string[] UserTags = [ "app-user", "app-user-list" ];
    public static readonly string[] ListTags = [ "app-user-list" ];

    public const string EntityName = "AppUser";
    public const string UnauthorizedAccessMessage = "User {0} is not authorized to access app user {1}.";
    public const string UnauthorizedExitControlAccessMessage = "User {0} is not authorized to access exit control {1}.";
    public const string AppUserNotFoundMessage = "App user with ID {0} not found.";
    public const string ExitControlNotFoundMessage = "Exit control with ID {0} not found.";
    public const string UnauthorizedAppUserAccessMessage = "User {0} is not authorized to access app user with id: {1}.";
    public const string UserIdEmptyMessage = "UserId cannot be empty";
    public const string RetrievedUserSearchResultsFromCacheMessage = "Retrieved app user search results from cache";
    public const string RetrievedSearchResultsFromCacheMessage = "Retrieved app users search results from cache";
}