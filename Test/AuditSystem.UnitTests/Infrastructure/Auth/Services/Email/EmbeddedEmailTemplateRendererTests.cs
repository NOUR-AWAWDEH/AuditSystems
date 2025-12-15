using System.Net;
using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using AuditSystem.Auth.Services.Email.Services.Renderers;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AuditSystem.UnitTests.Infrastructure.Auth.Services.Email;

public class EmbeddedEmailTemplateRendererTests
{
    private readonly ILogger<EmbeddedEmailTemplateRenderer> _logger;
    private readonly EmbeddedEmailTemplateRenderer _renderer;

    public EmbeddedEmailTemplateRendererTests()
    {
        _logger = A.Fake<ILogger<EmbeddedEmailTemplateRenderer>>();
        _renderer = new EmbeddedEmailTemplateRenderer(_logger);
    }

    [Fact]
    public void Render_ShouldThrowFileNotFoundException_WhenTemplateNotFound()
    {
        // Act & Assert
        FluentActions.Invoking(() => _renderer.Render("NonExistentTemplate", null))
            .Should().Throw<FileNotFoundException>()
            .WithMessage("*Embedded resource not found*NonExistentTemplate.html*");

        A.CallTo(_logger).Where(call =>
            call.Method.Name == "Log" &&
            call.GetArgument<LogLevel>(0) == LogLevel.Error &&
            call.GetArgument<object>(2)!.ToString()!.Contains("Email template not found")
        ).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public void Render_ShouldReturnTemplate_WhenExists()
    {
        // Act
        // "PasswordChanged" is known to exist from file system check
        var result = _renderer.Render("PasswordChanged", null);

        // Assert
        result.Should().NotBeNullOrWhiteSpace();
        result.Should().Contain("Password Changed"); // Basic check for content
    }

    [Fact]
    public void Render_ShouldReplacePlaceholders()
    {
        // Arrange
        var placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "UserName", new PlaceholderValue { Value = "TestUser", IsHtmlContent = false } },
            { "SupportEmail", new PlaceholderValue { Value = "support@test.com", IsHtmlContent = false } }
        };

        // Act
        var result = _renderer.Render("PasswordChanged", placeholders);

        // Assert
        result.Should().Contain("TestUser");
        result.Should().Contain("support@test.com");
        result.Should().NotContain("{{UserName}}");
        result.Should().NotContain("{{SupportEmail}}");
    }

    [Fact]
    public void Render_ShouldHtmlEncode_WhenIsHtmlContentIsFalse()
    {
        // Arrange
        var placeholders = new Dictionary<string, PlaceholderValue>
        {
            { "UserName", new PlaceholderValue { Value = "<script>alert('xss')</script>", IsHtmlContent = false } },
             { "SupportEmail", new PlaceholderValue { Value = "support@test.com", IsHtmlContent = false } }
        };


        // Act
        var result = _renderer.Render("PasswordChanged", placeholders);

        // Assert
        var expectedEncoded = WebUtility.HtmlEncode("<script>alert('xss')</script>");
        result.Should().Contain(expectedEncoded);
        result.Should().NotContain("<script>");
    }

    [Fact]
    public void Render_ShouldLogWarning_WhenUnreplacedPlaceholdersRemain()
    {
        // Arrange
        var placeholders = new Dictionary<string, PlaceholderValue>
        {
             // Missing "SupportEmail" or "UserName" depending on template
             { "UserName", new PlaceholderValue { Value = "TestUser", IsHtmlContent = false } }
        };

        // Act
        _renderer.Render("PasswordChanged", placeholders);

        // Assert
        A.CallTo(_logger).Where(call =>
            call.Method.Name == "Log" &&
            call.GetArgument<LogLevel>(0) == LogLevel.Warning &&
            call.GetArgument<object>(2)!.ToString()!.Contains("Unreplaced placeholders found")
        ).MustHaveHappened();
    }
}
