using System.Net;
using AuditSystem.BusinessLogic.Attributes;
using AuditSystem.BusinessLogic.Factories;
using AuditSystem.Contract.Models.RateLimiting;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AuditSystem.UnitTests.Attributes;

public class PreventBruteForceAttributeTests
{
    private readonly IServiceProvider _mockServiceProvider;
    private readonly IMemoryCache _memoryCache;
    private readonly ILogger<PreventBruteForceAttribute> _mockLogger;
    private readonly ActionExecutionDelegate _mockNext;
    private readonly DefaultHttpContext _httpContext;
    private readonly ActionExecutingContext _actionExecutingContext;

    public PreventBruteForceAttributeTests()
    {
        _mockServiceProvider = A.Fake<IServiceProvider>();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _mockLogger = A.Fake<ILogger<PreventBruteForceAttribute>>();
        _mockNext = A.Fake<ActionExecutionDelegate>();

        _httpContext = new DefaultHttpContext();
        _httpContext.Connection.RemoteIpAddress = IPAddress.Parse("127.0.0.1");
        _httpContext.RequestServices = _mockServiceProvider;

        _actionExecutingContext = new ActionExecutingContext(
            new ActionContext(
                _httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary()
            ),
            new List<IFilterMetadata>(),
            new Dictionary<string, object?>(),
            A.Fake<Controller>()
        );

        A.CallTo(() => _mockServiceProvider.GetService(typeof(IMemoryCache)))
            .Returns(_memoryCache);
        A.CallTo(() => _mockServiceProvider.GetService(typeof(ILogger<PreventBruteForceAttribute>)))
            .Returns(_mockLogger);

    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldCallNext_WhenRequestIsWithinLimit()
    {
        // Arrange
        var attribute = new PreventBruteForceAttribute(requestLimit: 5);

        // Act
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);

        // Assert
        A.CallTo(() => _mockNext()).MustHaveHappenedOnceExactly();

        // Verify cache entry created
        var cacheKey = "BruteForceProtection_127.0.0.1";
        _memoryCache.TryGetValue(cacheKey, out BruteForceProtectionEntry? entry).Should().BeTrue();
        entry.Should().NotBeNull();
        entry!.Requests.Should().Be(1);
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldBlock_WhenLimitExceeded()
    {
        // Arrange
        var limit = 2;
        var attribute = new PreventBruteForceAttribute(requestLimit: limit);

        // Simulate existing requests up to limit
        var cacheKey = "BruteForceProtection_127.0.0.1";
        var entry = new BruteForceProtectionEntry { Requests = limit };
        _memoryCache.Set(cacheKey, entry);

        // Act - Request limit + 1
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);

        // Assert
        A.CallTo(() => _mockNext()).MustNotHaveHappened();
        _actionExecutingContext.Result.Should().BeOfType<ContentResult>();

        var result = (ContentResult)_actionExecutingContext.Result!;
        result.StatusCode.Should().Be(429);

        // Verify entry updated
        _memoryCache.TryGetValue(cacheKey, out BruteForceProtectionEntry? updatedEntry);
        updatedEntry!.BlockCount.Should().Be(1);
        updatedEntry.BlockedUntil.Should().BeAfter(DateTime.UtcNow);
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldBlockImmediately_WhenAlreadyBlocked()
    {
        // Arrange
        var attribute = new PreventBruteForceAttribute();
        var cacheKey = "BruteForceProtection_127.0.0.1";
        var entry = new BruteForceProtectionEntry
        {
            BlockedUntil = DateTime.UtcNow.AddMinutes(5),
            Requests = 0
        };
        _memoryCache.Set(cacheKey, entry);

        // Act
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);

        // Assert
        A.CallTo(() => _mockNext()).MustNotHaveHappened();
        _actionExecutingContext.Result.Should().BeOfType<ContentResult>();

        var result = (ContentResult)_actionExecutingContext.Result!;
        result.StatusCode.Should().Be(429);
    }

    [Fact]
    public async Task OnActionExecutionAsync_ShouldHandleMultipleRequestsCorrectly()
    {
        // Arrange
        var limit = 3;
        var attribute = new PreventBruteForceAttribute(requestLimit: limit);

        // Act & Assert

        // Request 1
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);
        A.CallTo(() => _mockNext()).MustHaveHappenedOnceExactly();
        Fake.ClearRecordedCalls(_mockNext); // Reset calls

        // Request 2
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);
        A.CallTo(() => _mockNext()).MustHaveHappenedOnceExactly();
        Fake.ClearRecordedCalls(_mockNext);

        // Request 3
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);
        A.CallTo(() => _mockNext()).MustHaveHappenedOnceExactly();
        Fake.ClearRecordedCalls(_mockNext);

        // Request 4 (Should Block)
        // Reset context result for this call
        _actionExecutingContext.Result = null;
        await attribute.OnActionExecutionAsync(_actionExecutingContext, _mockNext);
        A.CallTo(() => _mockNext()).MustNotHaveHappened();

        _actionExecutingContext.Result.Should().BeOfType<ContentResult>()
             .Which.StatusCode.Should().Be(429);
    }
}
