namespace AuditSystem.Contract.Localization;

public static class LocalizationConstants
{
    public static readonly IReadOnlyDictionary<string, string> Languages = new Dictionary<string, string>
    {
        {EnglishShort, English}
    };

    public const string DefaultLanguage = English;

    public const string EnglishShort = "EN";
    public const string English = "English";

}