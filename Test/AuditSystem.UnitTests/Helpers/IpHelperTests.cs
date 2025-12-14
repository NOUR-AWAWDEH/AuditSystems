using System.Net;
using AuditSystem.BusinessLogic.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

using Xunit;

namespace AuditSystem.UnitTests.Helpers;

public class IpHelperTests
{
    [Fact]
    public void GetClientIp_ShouldReturnIp_WhenRemoteIpAddressIsPresent()
    {
        // Arrange
        var context = new DefaultHttpContext();
        var ipAddress = IPAddress.Parse("192.168.1.1");
        context.Connection.RemoteIpAddress = ipAddress;

        // Act
        var result = IpHelper.GetClientIp(context);

        // Assert
        result.Should().Be("192.168.1.1");
    }

    [Fact]
    public void GetClientIp_ShouldReturnUnknown_WhenRemoteIpAddressIsNull()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = null;

        // Act
        var result = IpHelper.GetClientIp(context);

        // Assert
        result.Should().Be("unknown");
    }

    [Fact]
    public void GetClientIp_ShouldReturnUnknown_WhenRemoteIpAddressIsNotPresent()
    {
        // Arrange
        var context = new DefaultHttpContext();
        context.Connection.RemoteIpAddress = null;

        // Act
        var result = IpHelper.GetClientIp(context);

        // Assert
        result.Should().Be("unknown");
    }
}
