using AuditSystem.Auth.Services.Email;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using Xunit;

namespace AuditSystem.UnitTests.Infrastructure.Auth.Services.Email;

public class EmailServiceTests
{
    private readonly IOptions<SmtpSettings> _smtpSettings;
    private readonly ILogger<EmailService> _logger;
    private readonly ISmtpClientFactory _smtpClientFactory;
    private readonly ISmtpClient _smtpClient;
    private readonly EmailService _emailService;

    public EmailServiceTests()
    {
        _smtpSettings = A.Fake<IOptions<SmtpSettings>>();
        _logger = A.Fake<ILogger<EmailService>>();
        _smtpClientFactory = A.Fake<ISmtpClientFactory>();
        _smtpClient = A.Fake<ISmtpClient>();

        var settings = new SmtpSettings
        {
            Host = "localhost",
            Port = 587,
            SenderAddress = "test@example.com",
            SenderName = "Test Sender",
            Password = "password",
            EnableSsl = true
        };

        A.CallTo(() => _smtpSettings.Value).Returns(settings);
        A.CallTo(() => _smtpClientFactory.CreateClient()).Returns(_smtpClient);

        _emailService = new EmailService(_smtpSettings, _logger, _smtpClientFactory);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentNullException_WhenDependenciesAreNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new EmailService(null!, _logger, _smtpClientFactory));
        Assert.Throws<ArgumentNullException>(() => new EmailService(_smtpSettings, null!, _smtpClientFactory));
        Assert.Throws<ArgumentNullException>(() => new EmailService(_smtpSettings, _logger, null!));
    }

    [Fact]
    public async Task SendEmailAsync_ShouldThrowArgumentNullException_WhenRequestIsNull()
    {
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentNullException>(() => _emailService.SendEmailAsync((EmailRequest)null!));
    }

    [Fact]
    public async Task SendEmailAsync_ShouldSendEmail_WhenRequestIsValid()
    {
        // Arrange
        var request = new EmailRequest
        {
            TemplateName = "PasswordChanged", // Use a known template or mock file system if possible. 
                                              // Assuming template exists as Embedded Resource for test simplicity or integration test.
                                              // Ideally, we'd mock the template loading if it wasn't embedded resource based.
                                              // Since it is embedded resource, we must use a real template name.
            ToEmails = new List<string> { "recipient@example.com" },
            Subject = "Test Subject",
            Placeholders = new Dictionary<string, PlaceholderValue>
            {
                { "UserName", new PlaceholderValue { Value = "TestUser", IsHtmlContent = false } },
                { "SupportEmail", new PlaceholderValue { Value = "support@example.com", IsHtmlContent = false } }
            }
        };

        // Act
        await _emailService.SendEmailAsync(request);

        // Assert
        A.CallTo(() => _smtpClientFactory.CreateClient()).MustHaveHappenedOnceExactly();
        A.CallTo(() => _smtpClient.SendMailAsync(A<MailMessage>.That.Matches(m =>
            m.Subject == "Test Subject" &&
            m.To[0].Address == "recipient@example.com" &&
            m.From!.Address == "test@example.com"
        ))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task SendPasswordChangedConfirmationAsync_ShouldSendEmailWithCorrectDetails()
    {
        // Arrange
        var email = "user@example.com";

        // Act
        await _emailService.SendPasswordChangedConfirmationAsync(email);

        // Assert
        A.CallTo(() => _smtpClient.SendMailAsync(A<MailMessage>.That.Matches(m =>
            m.To.Count == 1 &&
            m.To[0].Address == email &&
            m.Subject == "Your Password Has Been Changed" &&
            m.Body.Contains("support@AuditSystem.online") // Verify the specific change made by user
        ))).MustHaveHappenedOnceExactly();
    }
}