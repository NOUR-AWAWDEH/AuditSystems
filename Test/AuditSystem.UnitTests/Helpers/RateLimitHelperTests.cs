using AuditSystem.BusinessLogic.Helpers;
using AuditSystem.Contract.Models.RateLimiting;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Xunit;

namespace AuditSystem.UnitTests.Helpers;

public class RateLimitHelperTests
{
    private readonly ILogger _mockLogger;

    public RateLimitHelperTests()
    {
        _mockLogger = A.Fake<ILogger>();
    }

    [Fact]
    public void IsBlocked_ShouldReturnTrue_WhenBlockedUntilIsInFuture()
    {
        // Arrange
        var entry = new BruteForceProtectionEntry
        {
            BlockedUntil = DateTime.UtcNow.AddMinutes(5)
        };

        // Act
        var result = RateLimitHelper.IsBlocked(entry, _mockLogger, "127.0.0.1");

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsBlocked_ShouldReturnFalse_WhenBlockedUntilIsInPast()
    {
        // Arrange
        var entry = new BruteForceProtectionEntry
        {
            BlockedUntil = DateTime.UtcNow.AddMinutes(-1)
        };

        // Act
        var result = RateLimitHelper.IsBlocked(entry, _mockLogger, "127.0.0.1");

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void ApplyBlock_ShouldIncreaseBlockCountAndResetRequests()
    {
        // Arrange
        var entry = new BruteForceProtectionEntry
        {
            BlockCount = 0,
            Requests = 10,
            BlockedUntil = DateTime.MinValue
        };

        // Act
        RateLimitHelper.ApplyBlock(entry);

        // Assert
        entry.BlockCount.Should().Be(1);
        entry.Requests.Should().Be(0);
        entry.BlockedUntil.Should().BeAfter(DateTime.UtcNow);
    }
}