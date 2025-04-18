namespace AuditSystem.Application.Constants;

public static class CacheExpirations
{
    public static readonly TimeSpan ShortTerm = TimeSpan.FromMinutes(5); // Frequently changing data
    public static readonly TimeSpan MediumTerm = TimeSpan.FromHours(1); // Semi-static data    
    public static readonly TimeSpan LongTerm = TimeSpan.FromDays(1); // Rarely changing data
    public static readonly TimeSpan Configuration = TimeSpan.FromDays(7); // Configuration data
}