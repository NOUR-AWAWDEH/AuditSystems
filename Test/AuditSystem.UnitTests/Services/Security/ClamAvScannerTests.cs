using AuditSystem.Auth.Services.Security;
using AuditSystem.Contract.Interfaces.Security;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using nClam;
using Ardalis.Result;

namespace AuditSystem.UnitTests.Services.Security;

public class ClamAvScannerTests
{
    private readonly nClam.IClamClient _clamClient;
    private readonly ILogger<ClamAvScanner> _logger;
    private readonly ClamAvSettings _settings;
    private readonly IOptions<ClamAvSettings> _options;
    private ClamAvScanner _sut;

    public ClamAvScannerTests()
    {
        _clamClient = A.Fake<nClam.IClamClient>();
        _logger = A.Fake<ILogger<ClamAvScanner>>();
        _settings = new ClamAvSettings { Enabled = true, Host = "localhost", Port = 3310 };
        _options = Options.Create(_settings);
        _sut = new ClamAvScanner(_clamClient, _options, _logger);
    }

    [Fact]
    public async Task ScanStreamAsync_WhenDisabled_ShouldReturnSuccessWithoutScanning()
    {
        // Arrange
        _settings.Enabled = false;
        var stream = new MemoryStream();

        // Act
        var result = await _sut.ScanStreamAsync(stream, "test.pdf");

        // Assert
        result.IsSuccess.Should().BeTrue();
        A.CallTo(() => _clamClient.SendAndScanFileAsync(A<Stream>._, A<CancellationToken>._)).MustNotHaveHappened();
    }

    [Fact]
    public async Task ScanStreamAsync_WhenClean_ShouldReturnSuccess()
    {
        // Arrange
        var stream = new MemoryStream();
        // ClamScanResult takes the raw string from ClamAV and parses it.
        var scanResult = new ClamScanResult("stream: OK");

        A.CallTo(() => _clamClient.SendAndScanFileAsync(A<Stream>._, A<CancellationToken>._))
            .Returns(scanResult);

        // Act
        var result = await _sut.ScanStreamAsync(stream, "test.pdf");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task ScanStreamAsync_WhenVirusDetected_ShouldReturnError()
    {
        // Arrange
        var stream = new MemoryStream();
        var scanResult = new ClamScanResult("stream: Eicar-Test-Signature FOUND");

        A.CallTo(() => _clamClient.SendAndScanFileAsync(A<Stream>._, A<CancellationToken>._))
            .Returns(scanResult);

        // Act
        var result = await _sut.ScanStreamAsync(stream, "test.pdf");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Malware detected") && e.Contains("Eicar-Test-Signature"));
    }

    [Fact]
    public async Task ScanStreamAsync_WhenClamError_ShouldReturnError()
    {
        // Arrange
        var stream = new MemoryStream();
        var scanResult = new ClamScanResult("stream: ERROR");

        A.CallTo(() => _clamClient.SendAndScanFileAsync(A<Stream>._, A<CancellationToken>._))
            .Returns(scanResult);

        // Act
        var result = await _sut.ScanStreamAsync(stream, "test.pdf");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Error during malware scanning"));
    }

    [Fact]
    public async Task ScanStreamAsync_WhenExceptionOccurs_ShouldReturnError()
    {
        // Arrange
        var stream = new MemoryStream();
        A.CallTo(() => _clamClient.SendAndScanFileAsync(A<Stream>._, A<CancellationToken>._))
            .Throws(new Exception("Connection failed"));

        // Act
        var result = await _sut.ScanStreamAsync(stream, "test.pdf");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("service is currently unavailable"));
    }
}
