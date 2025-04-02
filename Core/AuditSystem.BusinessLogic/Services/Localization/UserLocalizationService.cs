using AuditSystem.Contract.Interfaces.Localization;
using AuditSystem.Contract.Localization;
using Microsoft.AspNetCore.Http;

namespace AuditSystem.BusinessLogic.Services.Localization;

internal sealed class UserLocalizationService( IHttpContextAccessor httpContextAccessor) : IUserLocalizationService
{
    public string GetUserLocalization()
    {
        var userLanguage = httpContextAccessor.HttpContext?.Request.Headers["User-Language"].ToString();
        return string.IsNullOrWhiteSpace(userLanguage) ?
            LocalizationConstants.DefaultLanguage :
            MapLanguageCodeToLanguage(userLanguage);
    }

    private static string MapLanguageCodeToLanguage(string code) =>
        LocalizationConstants.Languages.GetValueOrDefault(code.ToUpperInvariant(), LocalizationConstants.DefaultLanguage);
}