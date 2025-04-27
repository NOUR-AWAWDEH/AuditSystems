using AuditSystem.Application.Constants;
using AuditSystem.Contract.Interfaces.Localization;
using Microsoft.AspNetCore.OutputCaching;
using Microsoft.Extensions.Primitives;
using System.Text.RegularExpressions;

namespace AuditSystem.Host.Caching;

internal static partial class CacheConfiguration
{
    public static void AddCaching(this IServiceCollection services)
    {
        services.AddOutputCache(options =>
        {
            options.AddPolicy("TenantsCachePolicy", (builder) =>
            {
                builder.AddPolicy<TenantsCachePolicy>();
            });
        });

    }

    public partial class TenantsCachePolicy : IOutputCachePolicy
    {
        public ValueTask CacheRequestAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            var localizationService = context.HttpContext.RequestServices.GetService<IUserLocalizationService>();
            var localization = localizationService!.GetUserLocalization();
            var attemptOutputCaching = AttemptOutputCaching(context, localization);

            context.EnableOutputCaching = true;
            context.AllowCacheLookup = attemptOutputCaching;
            context.AllowCacheStorage = attemptOutputCaching;
            context.AllowLocking = true;

            context.CacheVaryByRules.VaryByValues.Add(new KeyValuePair<string, string>("user-language", localization)); // Split by Localization
            context.CacheVaryByRules.VaryByValues.Add(new KeyValuePair<string, string>("base-path", context.HttpContext.Request.Path)); // Split by Localization
            context.CacheVaryByRules.QueryKeys = "*";

            context.ResponseExpirationTimeSpan = TimeSpan.FromDays(1);//default

            if (context.HttpContext.Request.Path.Value is null)
            {
                return ValueTask.CompletedTask;
            }

            if (!attemptOutputCaching)
            {
                return ValueTask.CompletedTask;
            }

            var pattern = @"Id=(\d+)";
            var id = string.Empty;
            var match = Regex.Match(context.HttpContext.Request.QueryString.Value!, pattern);

            if (match.Success)
            {
                id = match.Groups[1].Value;
            }

            var prefix = context.HttpContext.Request.Path.Value.ToLowerInvariant().Contains("tenants");

            //Add route as a tag
            context.Tags.Add(string.Format(CacheKeys.CacheKey, prefix, id, localization));

            return ValueTask.CompletedTask;
        }

        public ValueTask ServeFromCacheAsync(OutputCacheContext context, CancellationToken cancellation)
        {
            return ValueTask.CompletedTask;
        }

        public ValueTask ServeResponseAsync
            (OutputCacheContext context, CancellationToken cancellationToken)
        {
            var response = context.HttpContext.Response;

            // Verify existence of cookie headers
            if (!StringValues.IsNullOrEmpty(response.Headers.SetCookie))
            {
                context.AllowCacheStorage = false;
                return ValueTask.CompletedTask;
            }

            // Check response code
            if (response.StatusCode == StatusCodes.Status200OK)
            {
                return ValueTask.CompletedTask;
            }

            context.AllowCacheStorage = false;
            return ValueTask.CompletedTask;

        }

        private static bool AttemptOutputCaching(OutputCacheContext context, string localization)
        {
            // Check if the current request fulfills the requirements
            // to be cached
            var request = context.HttpContext.Request;

            // Verify the method
            return HttpMethods.IsGet(request.Method);
        }
    }
}
