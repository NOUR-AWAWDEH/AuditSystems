using Ardalis.Result;
using AuditSystem.Application.Features.SupportingDocs.Create.Upload;
using AuditSystem.Contract.Interfaces.ModelServices.SupportingDocsServices;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace AuditSystem.UnitTests.Features.SupportingDocs;

public class UploadSupportingDocCommandHandlerTests
{
    private readonly ISupportingDocService _supportingDocService;
    private readonly ILogger<UploadSupportingDocCommandHandler> _logger;
    private readonly UploadSupportingDocCommandHandler _sut;

    public UploadSupportingDocCommandHandlerTests()
    {
        _supportingDocService = A.Fake<ISupportingDocService>();
        _logger = A.Fake<ILogger<UploadSupportingDocCommandHandler>>();
        _sut = new UploadSupportingDocCommandHandler(_supportingDocService, _logger);
    }

    #region Successful Upload Tests

    [Fact]
    public async Task Handle_WithValidFile_ShouldReturnSuccessResult()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Valid content");
        var userId = Guid.NewGuid();
        var expectedDocId = Guid.NewGuid();

        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = userId
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(mockFile, "test.pdf", userId))
            .Returns(Result.Created(expectedDocId));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value.SupportingDocId.Should().Be(expectedDocId);
        result.SuccessMessage.Should().Be("File uploaded successfully");
    }

    [Fact]
    public async Task Handle_WithValidFile_ShouldCallServiceWithCorrectParameters()
    {
        // Arrange
        var mockFile = CreateMockFile("document.docx", "Content");
        var userId = Guid.NewGuid();
        var fileName = "MyDocument.docx";

        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: fileName
        )
        {
            UserId = userId
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Returns(Result.Created(Guid.NewGuid()));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(mockFile, fileName, userId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Null/Empty File Tests

    [Fact]
    public async Task Handle_WithNullFile_ShouldReturnError()
    {
        // Arrange
        var command = new UploadSupportingDocCommand(
            File: null!,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("File cannot be empty"));
    }

    [Fact]
    public async Task Handle_WithEmptyFile_ShouldReturnError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", string.Empty);
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("File cannot be empty"));
    }

    [Fact]
    public async Task Handle_WithNullFile_ShouldLogWarning()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var command = new UploadSupportingDocCommand(
            File: null!,
            FileName: "test.pdf"
        )
        {
            UserId = userId
        };

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        A.CallTo(_logger)
            .Where(call => call.Method.Name == "Log" 
                && call.GetArgument<LogLevel>(0) == LogLevel.Warning)
            .MustHaveHappened();
    }

    #endregion

    #region Service Failure Tests

    [Fact]
    public async Task Handle_WhenServiceFails_ShouldReturnError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        var serviceError = "File storage failed";
        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Returns(Result.Error(serviceError));

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(serviceError);
    }

    [Fact]
    public async Task Handle_WhenServiceThrowsException_ShouldReturnError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Throws<InvalidOperationException>();

        // Act
        var result = await _sut.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Upload failed"));
    }

    [Fact]
    public async Task Handle_WhenServiceThrowsException_ShouldLogError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Throws<InvalidOperationException>();

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        A.CallTo(_logger)
            .Where(call => call.Method.Name == "Log" 
                && call.GetArgument<LogLevel>(0) == LogLevel.Error)
            .MustHaveHappened();
    }

    #endregion

    #region Database Constraint Tests

    [Fact]
    public async Task Handle_WithDuplicateDriveFileId_ShouldReturnFriendlyError()
    {
        // Arrange
        var file = CreateMockFile("test.pdf", "Content");
        var command = new UploadSupportingDocCommand(file, "test.pdf") { UserId = Guid.NewGuid() };
        
        // Use the testable subclass to simulate a duplicate key violation
        var testableHandler = new TestableUploadSupportingDocCommandHandler(_supportingDocService, _logger)
        {
            IsDuplicate = true
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Throws(new DbUpdateException("Error", new Exception("Inner")));

        // Act
        var result = await testableHandler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Errors.Should().Contain(e => e.Contains("Document storage conflict"));
    }

    #endregion

    #region Authorization Tests

    [Fact]
    public async Task Handle_ShouldUseUserIdFromCommand()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Content");
        var specificUserId = Guid.NewGuid();

        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = specificUserId
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Returns(Result.Created(Guid.NewGuid()));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(
            A<IFormFile>._,
            A<string>._,
            specificUserId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Security Edge Case Tests

    [Fact]
    public async Task Handle_WithMismatchedContentType_ShouldStillCallService()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Valid content", "application/x-msdownload"); // Spoofed as PDF but actually EXE mime
        var userId = Guid.NewGuid();

        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = userId
        };

        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(A<IFormFile>._, A<string>._, A<Guid>._))
            .Returns(Result.Created(Guid.NewGuid()));

        // Act
        await _sut.Handle(command, CancellationToken.None);

        // Assert
        // We ensure it calls the service, and the service (LocalFileStorage) will eventually catch the extension
        A.CallTo(() => _supportingDocService.CreateSupportingDocAsync(mockFile, "test.pdf", userId))
            .MustHaveHappenedOnceExactly();
    }

    #endregion

    #region Helper Methods

    private static IFormFile CreateMockFile(string fileName, string content, string contentType = "application/octet-stream")
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        var file = A.Fake<IFormFile>();

        A.CallTo(() => file.FileName).Returns(fileName);
        A.CallTo(() => file.Length).Returns(bytes.Length);
        A.CallTo(() => file.ContentType).Returns(contentType);
        A.CallTo(() => file.OpenReadStream()).Returns(stream);

        return file;
    }

    #endregion

    private class TestableUploadSupportingDocCommandHandler(
        ISupportingDocService supportingDocService,
        ILogger<UploadSupportingDocCommandHandler> logger)
        : UploadSupportingDocCommandHandler(supportingDocService, logger)
    {
        public bool IsDuplicate { get; set; }
        protected override bool IsDuplicateKeyException(DbUpdateException ex) => IsDuplicate;
    }
}