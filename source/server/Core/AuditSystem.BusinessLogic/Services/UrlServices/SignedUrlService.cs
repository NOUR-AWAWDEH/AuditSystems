using AuditSystem.Contract.Interfaces.FileStorage;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;

namespace AuditSystem.BusinessLogic.Services.UrlServices;

public class SignedUrlService : ISignedUrlService
{
    private readonly string _baseUrl;
    private readonly string _secret;

    public SignedUrlService(IConfiguration config)
    {
        _baseUrl = Environment.GetEnvironmentVariable("FILE_STORAGE_BASE_URL")
            ?? throw new InvalidOperationException("Missing FILE_STORAGE_BASE_URL");

        _secret = Environment.GetEnvironmentVariable("SIGNED_URL_SECRET_KEY")
            ?? throw new InvalidOperationException("Missing SIGNED_URL_SECRET_KEY");
    }

    public string GenerateSignedUrl(string filePath, TimeSpan validFor)
    {
        var expires = DateTimeOffset.UtcNow.Add(validFor).ToUnixTimeSeconds();
        var raw = $"{filePath}:{expires}";
        var sig = GenerateHmac(raw, _secret);
        return $"{_baseUrl.TrimEnd('/')}/download?file={Uri.EscapeDataString(filePath)}&expires={expires}&signature={Uri.EscapeDataString(sig)}";
    }

    public bool ValidateSignedUrl(string filePath, long expires, string signature)
    {
        if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() > expires)
            return false;

        var expected = GenerateHmac($"{filePath}:{expires}", _secret);
        return ConstantTimeCompare(signature, expected);
    }

    private static string GenerateHmac(string data, string key)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
        return Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(data)));
    }

    private static bool ConstantTimeCompare(string a, string b)
    {
        var aBytes = Encoding.UTF8.GetBytes(a);
        var bBytes = Encoding.UTF8.GetBytes(b);
        if (aBytes.Length != bBytes.Length) return false;

        int result = 0;
        for (int i = 0; i < aBytes.Length; i++)
            result |= aBytes[i] ^ bBytes[i];
        return result == 0;
    }
}
