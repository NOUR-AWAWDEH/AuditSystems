using System.Net.Mail;
using AuditSystem.Auth.Services.Email.Abstractions;
using AuditSystem.Auth.Services.Email.Models;
using AuditSystem.Auth.Services.Email.Services.Smtp;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace AuditSystem.UnitTests.Infrastructure.Auth.Services.Email;

public class SmtpEmailSenderTests
{
    private readonly SmtpSettings _settings;
    private readonly ISmtpClientFactory _factory;
    private readonly ISmtpClient _smtpClient;
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly SmtpEmailSender _sender;

    public SmtpEmailSenderTests()
    {
        _settings = new SmtpSettings
        {
            Host = "localhost",
            Port = 25,
            SenderAddress = "test@example.com",
            SenderName = "Sender"
        };
        var options = Options.Create(_settings);

        _factory = A.Fake<ISmtpClientFactory>();
        _smtpClient = A.Fake<ISmtpClient>();
        _logger = A.Fake<ILogger<SmtpEmailSender>>();

        A.CallTo(() => _factory.CreateClient()).Returns(_smtpClient);

        _sender = new SmtpEmailSender(options, _factory, _logger);
    }

    [Fact]
    public async Task SendAsync_ShouldThrowArgumentException_WhenRecipientsEmpty()
    {
        var message = new EmailMessage { To = new(), Subject = "S", Body = "B" };
        await FluentActions.Awaiting(() => _sender.SendAsync(message))
            .Should().ThrowAsync<ArgumentException>()
            .WithMessage("*Recipient list*");
    }

    [Fact]
    public async Task SendAsync_ShouldThrowArgumentException_WhenNoValidRecipients()
    {
        var message = new EmailMessage { To = new() { " ", null! }, Subject = "S", Body = "B" };
        await FluentActions.Awaiting(() => _sender.SendAsync(message))
           .Should().ThrowAsync<ArgumentException>()
           .WithMessage("*No valid recipients*");
    }

    [Fact]
    public async Task SendAsync_ShouldSendEmail_WhenValid()
    {
        var message = new EmailMessage
        {
            To = new() { "recipient@test.com" },
            Subject = "Subject",
            Body = "Body"
        };

        await _sender.SendAsync(message);

        A.CallTo(() => _smtpClient.SendMailAsync(A<MailMessage>.That.Matches(m =>
            m.Subject == "Subject" &&
            m.Body == "Body" &&
            m.To[0].Address == "recipient@test.com" &&
            m.From!.Address == "test@example.com"
        ))).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task SendAsync_ShouldWrapSmtpException()
    {
        var message = new EmailMessage
        {
            To = new() { "recipient@test.com" },
            Subject = "Subject",
            Body = "Body"
        };

        A.CallTo(() => _smtpClient.SendMailAsync(A<MailMessage>._))
            .Throws(new SmtpException("Fail"));

        var ex = await FluentActions.Awaiting(() => _sender.SendAsync(message))
            .Should().ThrowAsync<Exception>()
            .WithMessage("SMTP send failed");

        ex.Which.InnerException.Should().BeOfType<SmtpException>();
    }
}
