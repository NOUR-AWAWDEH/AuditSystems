using nClam;
using Ardalis.Result;
using AuditSystem.Contract.Interfaces.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AuditSystem.Auth.Services.Security;

public class ClamAvScanner(
    IClamClient clamClient,
    IOptions<ClamAvSettings> settings,
    ILogger<ClamAvScanner> logger) : IMalwareScanner
{
    private readonly IClamClient _clamClient = clamClient;
    private readonly ClamAvSettings _settings = settings.Value;
    private readonly ILogger<ClamAvScanner> _logger = logger;

    public async Task<Result> ScanStreamAsync(Stream fileStream, string fileName, CancellationToken cancellationToken = default)
    {
        if (!_settings.Enabled)
        {
            _logger.LogInformation("ClamAV scanning is disabled. Skipping scan for {FileName}.", fileName);
            return Result.Success();
        }

        try
        {
            var scanResult = await _clamClient.SendAndScanFileAsync(fileStream, cancellationToken);

            switch (scanResult.Result)
            {
                case ClamScanResults.Clean:
                    _logger.LogInformation("File {FileName} scanned successfully. No malware detected.", fileName);
                    return Result.Success();
                case ClamScanResults.VirusDetected:
                    _logger.LogWarning("Malware detected in file {FileName}: {RawResult}", fileName, scanResult.RawResult);
                    var virusName = scanResult.InfectedFiles?.FirstOrDefault()?.VirusName ?? "Unknown virus";
                    return Result.Error($"Malware detected: {virusName}");
                case ClamScanResults.Error:
                    _logger.LogError("ClamAV error while scanning {FileName}: {RawResult}", fileName, scanResult.RawResult);
                    return Result.Error("Error during malware scanning.");
                case ClamScanResults.Unknown:
                default:
                    _logger.LogWarning("Unknown ClamAV result while scanning {FileName}: {RawResult}", fileName, scanResult.RawResult);
                    return Result.Error("Unknown malware scanning result.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to connect to ClamAV server at {Host}:{Port} while scanning {FileName}.", _settings.Host, _settings.Port, fileName);
            return Result.Error("Malware scanning service is currently unavailable.");
        }
    }
}
