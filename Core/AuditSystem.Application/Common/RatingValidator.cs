namespace AuditSystem.Application.Common;

public static class RatingValidator
{
    public static readonly string[] ValidLevels = ["High", "Medium", "Low", "Critical", "Minimal"];
    
    public static bool IsValidLevel(string level)
    {
        return ValidLevels.Contains(level, StringComparer.OrdinalIgnoreCase);
    }
}