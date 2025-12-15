using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using AuditSystem.Auth.Services.Email.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AuditSystem.UnitTests.Infrastructure.Auth.Services.Email;

public class EmailServiceTests
{
    private readonly IEmailTemplateRenderer _renderer;
    private readonly IEmailSender _sender;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _renderer = A.Fake<IEmailTemplateRenderer>();
        _sender = A.Fake<IEmailSender>();
        _emailService = new EmailService(_renderer, _sender);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        FluentActions.Invoking(() => new EmailService(null!, _sender))
            .Should().Throw<ArgumentNullException>();

        FluentActions.Invoking(() => new EmailService(_renderer, null!))
            .Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public async Task SendEmailAsync_Request_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        await FluentActions.Awaiting(() => _emailService.SendEmailAsync((EmailRequest)null!))
            .Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task SendEmailAsync_Request_ShouldValidateFields()
    {
        var request = new EmailRequest { TemplateName = "", Subject = "S", ToEmails = new() { "a@b.c" } };
        await FluentActions.Awaiting(() => _emailService.SendEmailAsync(request))
            .Should().ThrowAsync<ArgumentException>().WithMessage("*Template name*");

        request = new EmailRequest { TemplateName = "T", Subject = "", ToEmails = new() { "a@b.c" } };
        await FluentActions.Awaiting(() => _emailService.SendEmailAsync(request))
            .Should().ThrowAsync<ArgumentException>().WithMessage("*Subject*");

        request = new EmailRequest { TemplateName = "T", Subject = "S", ToEmails = null! };
        await FluentActions.Awaiting(() => _emailService.SendEmailAsync(request))
            .Should().ThrowAsync<ArgumentException>().WithMessage("*recipient*");
    }

    [Fact]
    public async Task SendEmailAsync_Request_ShouldRenderAndSend()
    {
        // Arrange
        var request = new EmailRequest
        {
            TemplateName = "TestTemplate",
            Subject = "Test Subject",
            ToEmails = new List<string> { "test@example.com" },
            Placeholders = new Dictionary<string, PlaceholderValue>()
        };

        A.CallTo(() => _renderer.Render("TestTemplate", request.Placeholders))
            .Returns("Rendered Body");

        // Act
        await _emailService.SendEmailAsync(request);

        // Assert
        A.CallTo(() => _sender.SendAsync(A<EmailMessage>.That.Matches(m =>
            m.Subject == "Test Subject" &&
            m.Body == "Rendered Body" &&
            m.To.Count == 1 &&
            m.To[0] == "test@example.com"
        ))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task SendEmailAsync_Direct_ShouldSend()
    {
        // Act
        await _emailService.SendEmailAsync("test@example.com", "Subject", "Body", isHtml: false);

        // Assert
        A.CallTo(() => _sender.SendAsync(A<EmailMessage>.That.Matches(m =>
            m.Subject == "Subject" &&
            m.Body == "Body" &&
            m.IsHtml == false &&
            m.To.Count == 1 &&
            m.To[0] == "test@example.com"
        ))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task SendPasswordResetEmailAsync_ShouldRenderAndSend()
    {
        // Arrange
        A.CallTo(() => _renderer.Render("PasswordReset", A<Dictionary<string, PlaceholderValue>>._))
            .Returns("Reset Body");

        // Act
        await _emailService.SendPasswordResetEmailAsync("user@example.com", "http://reset.link");

        // Assert
        A.CallTo(() => _renderer.Render("PasswordReset", A<Dictionary<string, PlaceholderValue>>.That.Matches(p =>
            p["ResetUrl"].Value == "http://reset.link" &&
            p["UserName"].Value == "user"
        ))).MustHaveHappenedOnceExactly();

        A.CallTo(() => _sender.SendAsync(A<EmailMessage>.That.Matches(m =>
            m.Subject == "Reset Your Password" && // Assumed based on logic or implementation detail
            m.Body == "Reset Body" &&
            m.To[0] == "user@example.com"
        ))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task SendPasswordChangedConfirmationAsync_ShouldRenderAndSend()
    {
        // Arrange
        A.CallTo(() => _renderer.Render("PasswordChanged", A<Dictionary<string, PlaceholderValue>>._))
            .Returns("Changed Body");

        // Act
        await _emailService.SendPasswordChangedConfirmationAsync("user@example.com");

        // Assert
        A.CallTo(() => _renderer.Render("PasswordChanged", A<Dictionary<string, PlaceholderValue>>.That.Matches(p =>
            p.ContainsKey("SupportEmail")
        ))).MustHaveHappenedOnceExactly();

        A.CallTo(() => _sender.SendAsync(A<EmailMessage>.That.Matches(m =>
            m.Body == "Changed Body" &&
            m.To[0] == "user@example.com"
        ))).MustHaveHappenedOnceExactly();
    }
}