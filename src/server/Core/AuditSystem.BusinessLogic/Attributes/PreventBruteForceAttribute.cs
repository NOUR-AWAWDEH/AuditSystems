using AuditSystem.BusinessLogic.Factories;
using AuditSystem.BusinessLogic.Helpers;
using AuditSystem.BusinessLogic.Logging;
using AuditSystem.Contract.Models.RateLimiting;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AuditSystem.BusinessLogic.Attributes;

[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
public sealed class PreventBruteForceAttribute : Attribute, IAsyncActionFilter
{
    private const int DefaultRequestLimit = 20;
    private static readonly SemaphoreSlim _semaphore = new(1, 1);

    private readonly int _requestLimit;

    public PreventBruteForceAttribute(int requestLimit = DefaultRequestLimit)
    {
        _requestLimit = requestLimit > 0 ? requestLimit : DefaultRequestLimit;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var serviceProvider = context.HttpContext.RequestServices;
        var cache = serviceProvider.GetRequiredService<IMemoryCache>();
        var logger = serviceProvider.GetService<ILogger<PreventBruteForceAttribute>>();
        var ip = IpHelper.GetClientIp(context.HttpContext);

        await _semaphore.WaitAsync();
        try
        {
            var cacheKey = $"BruteForceProtection_{ip}";

            if (!cache.TryGetValue(cacheKey, out BruteForceProtectionEntry entry))
            {
                entry = new BruteForceProtectionEntry();
                cache.Set(cacheKey, entry, CacheOptionsFactory.CreateBruteForceProtectionOptions());
            }

            if (RateLimitHelper.IsBlocked(entry, logger, ip))
            {
                context.Result = ResponseFactory.CreateBlockedResponse(entry.BlockedUntil);
                RateLimitLogger.LogBlockedAttempt(logger, ip, entry);
                return;
            }

            entry.Requests++;
            RateLimitLogger.LogRequest(logger, ip, entry);

            if (entry.Requests > _requestLimit)
            {
                RateLimitHelper.ApplyBlock(entry);
                cache.Set(cacheKey, entry, CacheOptionsFactory.CreateBruteForceProtectionOptions());
                context.Result = ResponseFactory.CreateBlockedResponse(entry.BlockedUntil);
                RateLimitLogger.LogRateLimitApplied(logger, ip, entry);
                return;
            }

            cache.Set(cacheKey, entry, CacheOptionsFactory.CreateBruteForceProtectionOptions());
        }
        finally
        {
            _semaphore.Release();
        }

        await next();
    }
}