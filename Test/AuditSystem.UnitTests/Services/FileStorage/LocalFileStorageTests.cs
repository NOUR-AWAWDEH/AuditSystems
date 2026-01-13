using AuditSystem.BusinessLogic.Services.FileStorageServices;
using AuditSystem.Contract.Interfaces.FileStorage;
using AuditSystem.Contract.Interfaces.Security;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Ardalis.Result;
using System.IO.Compression;

namespace AuditSystem.UnitTests.Services.FileStorage;

public class LocalFileStorageTests : IDisposable
{
    private readonly IFilePathBuilder _pathBuilder;
    private readonly IMalwareScanner _malwareScanner;
    private readonly ILogger<LocalFileStorage> _logger;
    private readonly LocalFileStorage _sut;
    private readonly string _testDirectory;

    public LocalFileStorageTests()
    {
        _pathBuilder = A.Fake<IFilePathBuilder>();
        _malwareScanner = A.Fake<IMalwareScanner>();
        _logger = A.Fake<ILogger<LocalFileStorage>>();
        _sut = new LocalFileStorage(_pathBuilder, _malwareScanner, _logger);
        
        // Default to clean scan
        A.CallTo(() => _malwareScanner.ScanStreamAsync(A<Stream>._, A<string>._, A<CancellationToken>._))
            .Returns(Result.Success());

        // Setup test directory
        _testDirectory = Path.Combine(Path.GetTempPath(), "FileUploadTests", Guid.NewGuid().ToString());
        Directory.CreateDirectory(_testDirectory);
        
        // Configure path builder to return test directory paths
        A.CallTo(() => _pathBuilder.GetFullPath(A<Guid>._, A<string>._, A<string>._))
            .ReturnsLazily((Guid userId, string serviceName, string fileName) =>
                Path.Combine(_testDirectory, fileName));
    }

    #region File Extension Validation Tests

    [Theory]
    [InlineData(".pdf")]
    [InlineData(".docx")]
    [InlineData(".doc")]
    [InlineData(".xlsx")]
    [InlineData(".xls")]
    [InlineData(".pptx")]
    [InlineData(".ppt")]
    [InlineData(".rtf")]
    public async Task SaveFileAsync_WithAllowedExtension_ShouldSucceed(string extension)
    {
        // Arrange
        var file = CreateMockFile($"test{extension}", "Test content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.SavedFileName.Should().EndWith(extension);
    }

    [Theory]
    [InlineData(".exe", "executable")]
    [InlineData(".bat", "batch file")]
    [InlineData(".sh", "shell script")]
    [InlineData(".php", "PHP script")]
    [InlineData(".asp", "ASP file")]
    [InlineData(".aspx", "ASPX file")]
    [InlineData(".js", "JavaScript")]
    [InlineData(".vbs", "VBScript")]
    [InlineData(".jar", "Java archive")]
    [InlineData(".py", "Python script")]
    public async Task SaveFileAsync_WithDisallowedExtension_ShouldFail(string extension, string description)
    {
        // Arrange
        var file = CreateMockFile($"malicious{extension}", "Malicious content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Invalid file type"));
    }

    [Theory]
    [InlineData("file.PDF")]
    [InlineData("file.DOCX")]
    [InlineData("file.Pdf")]
    [InlineData("file.XlSx")]
    public async Task SaveFileAsync_WithMixedCaseExtension_ShouldSucceed(string fileName)
    {
        // Arrange
        var file = CreateMockFile(fileName, "Test content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task SaveFileAsync_WithDoubleExtension_ShouldRejectBasedOnActualExtension()
    {
        // Arrange - file appears to be PDF but is actually EXE
        var file = CreateMockFile("document.pdf.exe", "Malicious content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Invalid file type"));
    }

    [Fact]
    public async Task SaveFileAsync_WithNoExtension_ShouldFail()
    {
        // Arrange
        var file = CreateMockFile("filewithoutext", "Test content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Invalid file type"));
    }

    #endregion

    #region File Size Validation Tests

    [Fact]
    public async Task SaveFileAsync_WithFileSizeUnderLimit_ShouldSucceed()
    {
        // Arrange - 100MB file content + 4 bytes magic
        var file = CreateMockFile("test.pdf", new string('A', 100 * 1024 * 1024));
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.FileSize.Should().Be(100 * 1024 * 1024 + 4); // Include 4 bytes magic
    }

    [Fact]
    public async Task SaveFileAsync_WithFileSizeAtLimit_ShouldSucceed()
    {
        // Arrange - Exactly 200MB (including magic bytes)
        var file = CreateMockFile("test.pdf", new string('A', 200 * 1024 * 1024 - 4));
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task SaveFileAsync_WithFileSizeOverLimit_ShouldFail()
    {
        // Arrange - 201MB file (over limit)
        var file = CreateMockFile("test.pdf", new string('A', 201 * 1024 * 1024));
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("too large"));
    }

    [Fact]
    public async Task SaveFileAsync_WithZeroByteFile_ShouldFail()
    {
        // Arrange - Skip magic bytes for this test
        var file = CreateMockFile("test.pdf", string.Empty, includeMagic: false);
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("No file uploaded"));
    }

    #endregion

    #region Filename Sanitization Tests

    [Theory]
    [InlineData("../../../etc/passwd", "passwd")]
    [InlineData("..\\..\\..\\windows\\system32\\config", "config")]
    [InlineData("../../../../root/.ssh/id_rsa", "id_rsa")]
    [InlineData("%2e%2e%2f%2e%2e%2fetc%2fpasswd", "2e_2e_2f_2e_2e_2fetc_2fpasswd")]
    [InlineData("%252e%252e%252fetc%252fpasswd", "252e_252e_252fetc_252fpasswd")]
    public async Task SaveFileAsync_WithPathTraversalAttempt_ShouldSanitizeFilename(string maliciousName, string expectedSanitized)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", maliciousName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().Contain(expectedSanitized);
        result.Value.SavedFileName.Should().NotContain("..");
        result.Value.SavedFileName.Should().NotContain("/");
        result.Value.SavedFileName.Should().NotContain("\\");
    }

    [Theory]
    [InlineData("file<>name.pdf", "file__name")]
    [InlineData("file:name.pdf", "file_name")]
    [InlineData("file\"name.pdf", "file_name")]
    [InlineData("file|name.pdf", "file_name")]
    [InlineData("file?name.pdf", "file_name")]
    [InlineData("file*name.pdf", "file_name")]
    public async Task SaveFileAsync_WithSpecialCharacters_ShouldSanitize(string fileName, string expectedBase)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", fileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().Contain(expectedBase);
        result.Value.SavedFileName.Should().NotContain("<");
        result.Value.SavedFileName.Should().NotContain(">");
        result.Value.SavedFileName.Should().NotContain(":");
        result.Value.SavedFileName.Should().NotContain("\"");
        result.Value.SavedFileName.Should().NotContain("|");
        result.Value.SavedFileName.Should().NotContain("?");
        result.Value.SavedFileName.Should().NotContain("*");
    }

    [Theory]
    [InlineData("file.pdf ", "file")]
    [InlineData("file.pdf.", "file")]
    [InlineData("file..pdf", "file")]
    [InlineData("file   name.pdf", "file___name")]
    [InlineData("...pdf", "file")]
    public async Task SaveFileAsync_WithTrailingDotsOrSpaces_ShouldSanitize(string fileName, string expectedBase)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", fileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().Contain(expectedBase);
        result.Value.SavedFileName.Should().NotContain("..");
    }

    [Fact]
    public async Task SaveFileAsync_WithUnicodeCharacters_ShouldHandleGracefully()
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();
        var unicodeFileName = "文档测试.pdf"; // Chinese characters

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", unicodeFileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task SaveFileAsync_WithExtremelyLongFilename_ShouldHandleGracefully()
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();
        var longFileName = new string('A', 300) + ".pdf"; // 300+ characters

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", longFileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().NotBeNullOrEmpty();
    }

    [Theory]
    [InlineData("!@#$%^&*().pdf")]
    [InlineData("<<>>&&||.pdf")]
    [InlineData(";;;;;;;;.pdf")]
    public async Task SaveFileAsync_WithOnlySpecialCharacters_ShouldUseFallbackName(string fileName)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", fileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().Contain("file"); // Fallback name
    }

    [Fact]
    public async Task SaveFileAsync_WithEmptyFilename_ShouldUseFallbackName()
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().Contain("file");
    }

    #endregion

    #region Malicious Pattern Detection Tests

    [Theory]
    [InlineData("file.pdf\0.exe")]
    [InlineData("document\0.bat")]
    public async Task SaveFileAsync_WithNullByteInjection_ShouldSanitize(string maliciousFileName)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", maliciousFileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().NotContain("\0");
    }

    [Theory]
    [InlineData("file\r\nname.pdf")]
    [InlineData("file\rname.pdf")]
    [InlineData("file\nname.pdf")]
    public async Task SaveFileAsync_WithCRLFInjection_ShouldSanitize(string maliciousFileName)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", maliciousFileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().NotContain("\r");
        result.Value.SavedFileName.Should().NotContain("\n");
    }

    [Theory]
    [InlineData("'; DROP TABLE Users; --.pdf")]
    [InlineData("1' OR '1'='1.pdf")]
    public async Task SaveFileAsync_WithSQLInjectionPattern_ShouldSanitize(string maliciousFileName)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", maliciousFileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().NotContain("'");
        result.Value.SavedFileName.Should().NotContain(";");
    }

    [Theory]
    [InlineData("<script>alert('xss')</script>.pdf")]
    [InlineData("javascript:alert(1).pdf")]
    public async Task SaveFileAsync_WithScriptInjectionPattern_ShouldSanitize(string maliciousFileName)
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", maliciousFileName);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().NotContain("<");
        result.Value.SavedFileName.Should().NotContain(">");
        result.Value.SavedFileName.Should().NotContain(":");
    }

    #endregion

    #region Null/Empty File Tests

    [Fact]
    public async Task SaveFileAsync_WithNullFile_ShouldFail()
    {
        // Arrange
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(null!, userId, "TestService", "testfile");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("No file uploaded"));
    }

    #endregion

    #region Unique Filename Generation Tests

    [Fact]
    public async Task SaveFileAsync_ShouldGenerateUniqueFilenames()
    {
        // Arrange
        var file1 = CreateMockFile("test.pdf", "Content 1");
        var file2 = CreateMockFile("test.pdf", "Content 2");
        var userId = Guid.NewGuid();

        // Act
        var result1 = await _sut.SaveFileAsync(file1, userId, "TestService", "document");
        var result2 = await _sut.SaveFileAsync(file2, userId, "TestService", "document");

        // Assert
        result1.IsSuccess.Should().BeTrue();
        result2.IsSuccess.Should().BeTrue();
        result1.Value.SavedFileName.Should().NotBe(result2.Value.SavedFileName);
    }

    [Fact]
    public async Task SaveFileAsync_ShouldIncludeGuidInFilename()
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "document");

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.SavedFileName.Should().MatchRegex(@"document_[a-f0-9]{32}\.pdf");
    }

    #endregion

    #region Delete File Tests

    [Fact]
    public async Task DeleteFileAsync_WithValidPath_ShouldSucceed()
    {
        // Arrange
        var testFilePath = Path.Combine(_testDirectory, "testfile.pdf");
        await File.WriteAllTextAsync(testFilePath, "Test content");

        // Act
        var result = await _sut.DeleteFileAsync(testFilePath);

        // Assert
        result.IsSuccess.Should().BeTrue();
        File.Exists(testFilePath).Should().BeFalse();
    }

    [Fact]
    public async Task DeleteFileAsync_WithNonExistentFile_ShouldFail()
    {
        // Arrange
        var nonExistentPath = Path.Combine(_testDirectory, "nonexistent.pdf");

        // Act
        var result = await _sut.DeleteFileAsync(nonExistentPath);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("does not exist"));
    }

    [Fact]
    public async Task DeleteFileAsync_WithEmptyPath_ShouldFail()
    {
        // Act
        var result = await _sut.DeleteFileAsync(string.Empty);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Invalid file path"));
    }

    [Fact]
    public async Task SaveFileAsync_WithInfectedFile_ShouldFail()
    {
        // Arrange
        // Use valid PDF header so it passes Magic Byte validation
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        var file = CreateMockFileWithBytes("virus.pdf", pdfBytes);
        var userId = Guid.NewGuid();
        
        A.CallTo(() => _malwareScanner.ScanStreamAsync(A<Stream>._, A<string>._, A<CancellationToken>._))
            .Returns(Result.Error("Malware detected."));

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "virus.pdf");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Malware detected"));
        
        // Verify file was NOT saved
        var files = Directory.GetFiles(_testDirectory);
        files.Should().BeEmpty();
    }

    [Fact]
    public async Task SaveFileAsync_WithMagicByteMismatch_ShouldFail()
    {
        // Arrange - Explicitly provide mismatched bytes
        var invalidBytes = new byte[] { 0x00, 0x00, 0x00, 0x00 };
        var file = CreateMockFileWithBytes("test.pdf", invalidBytes);
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "test.pdf");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("File content does not match its extension"));
    }

    [Fact]
    public async Task SaveFileAsync_WithValidMagicBytes_ShouldSucceed()
    {
        // Arrange - Real PDF magic bytes: %PDF
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x31, 0x2E, 0x37 };
        var file = CreateMockFileWithBytes("test.pdf", pdfBytes);
        var userId = Guid.NewGuid();

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "test.pdf");

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task SaveFileAsync_WithScanTimeout_ShouldFail()
    {
        // Arrange
        var pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46 };
        var file = CreateMockFileWithBytes("test.pdf", pdfBytes);
        var userId = Guid.NewGuid();

        A.CallTo(() => _malwareScanner.ScanStreamAsync(A<Stream>._, A<string>._, A<CancellationToken>._))
            .ThrowsAsync(new OperationCanceledException());

        // Act
        var result = await _sut.SaveFileAsync(file, userId, "TestService", "test.pdf");

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("scanning service timed out"));
    }

    #endregion

    #region Helper Methods

    private static bool IsOpenXml(string ext) =>
        ext.EndsWith("x") || ext.EndsWith("m");

    private static IFormFile CreateMockFile(string fileName, string content, bool includeMagic = true)
    {
        var ext = Path.GetExtension(fileName).ToLowerInvariant();
        byte[] bytes;

        if (IsOpenXml(ext))
        {
            using var ms = new MemoryStream();
            using (var zip = new ZipArchive(ms, ZipArchiveMode.Create, true))
            {
                zip.CreateEntry("[Content_Types].xml");
                if (ext.Contains(".doc")) zip.CreateEntry("word/document.xml");
                else if (ext.Contains(".xls")) zip.CreateEntry("xl/workbook.xml");
                else if (ext.Contains(".ppt")) zip.CreateEntry("ppt/presentation.xml");
            }
            bytes = ms.ToArray();
        }
        else
        {
            bytes = System.Text.Encoding.UTF8.GetBytes(content);

            if (includeMagic)
            {
                byte[] magicBytes = ext switch
                {
                    ".pdf" => [0x25, 0x50, 0x44, 0x46],
                    ".doc" or ".xls" or ".ppt" => [0xD0, 0xCF, 0x11, 0xE0, 0xA1, 0xB1, 0x1A, 0xE1],
                    ".rtf" => [0x7B, 0x5C, 0x72, 0x74, 0x66, 0x31],
                    _ => []
                };

                if (magicBytes.Length > 0)
                {
                    var newBytes = new byte[magicBytes.Length + bytes.Length];
                    Buffer.BlockCopy(magicBytes, 0, newBytes, 0, magicBytes.Length);
                    Buffer.BlockCopy(bytes, 0, newBytes, magicBytes.Length, bytes.Length);
                    bytes = newBytes;
                }
            }
        }

        var file = A.Fake<IFormFile>();
        
        A.CallTo(() => file.FileName).Returns(fileName);
        A.CallTo(() => file.Length).Returns(bytes.Length);
        A.CallTo(() => file.OpenReadStream()).ReturnsLazily(() => new MemoryStream(bytes));
        A.CallTo(() => file.CopyToAsync(A<Stream>._, A<CancellationToken>._))
            .ReturnsLazily((Stream targetStream, CancellationToken _) =>
            {
                using var sourceStream = new MemoryStream(bytes);
                sourceStream.CopyTo(targetStream);
                return Task.CompletedTask;
            });

        return file;
    }

    private static IFormFile CreateMockFileWithBytes(string fileName, byte[] bytes)
    {
        var file = A.Fake<IFormFile>();
        
        A.CallTo(() => file.FileName).Returns(fileName);
        A.CallTo(() => file.Length).Returns(bytes.Length);
        A.CallTo(() => file.OpenReadStream()).ReturnsLazily(() => new MemoryStream(bytes));
        A.CallTo(() => file.CopyToAsync(A<Stream>._, A<CancellationToken>._))
            .ReturnsLazily((Stream targetStream, CancellationToken _) =>
            {
                using var sourceStream = new MemoryStream(bytes);
                sourceStream.CopyTo(targetStream);
                return Task.CompletedTask;
            });

        return file;
    }

    public void Dispose()
    {
        // Cleanup test directory
        if (Directory.Exists(_testDirectory))
        {
            Directory.Delete(_testDirectory, true);
        }
    }

    #endregion
}
