using AuditSystem.Application.Features.SupportingDocs.Create.Upload;
using FluentAssertions;
using FluentValidation.TestHelper;
using Microsoft.AspNetCore.Http;
using FakeItEasy;

namespace AuditSystem.UnitTests.Features.SupportingDocs;

public class UploadSupportingDocCommandValidatorTests
{
    private readonly UploadSupportingDocCommandValidator _validator;

    public UploadSupportingDocCommandValidatorTests()
    {
        _validator = new UploadSupportingDocCommandValidator();
    }

    #region File Presence Validation Tests

    [Fact]
    public void Validate_WithNullFile_ShouldHaveValidationError()
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
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File)
            .WithErrorMessage("File is required.");
    }

    [Fact]
    public void Validate_WithValidFile_ShouldNotHaveValidationErrorForFile()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.File);
    }

    #endregion

    #region File Size Validation Tests

    [Fact]
    public void Validate_WithEmptyFile_ShouldHaveValidationError()
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
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.File.Length)
            .WithErrorMessage("File cannot be empty.");
    }

    [Fact]
    public void Validate_WithNonEmptyFile_ShouldNotHaveValidationErrorForFileLength()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Some content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.File.Length);
    }

    #endregion

    #region Filename Validation Tests

    [Fact]
    public void Validate_WithEmptyFileName_ShouldHaveValidationError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: string.Empty
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FileName);
    }

    [Fact]
    public void Validate_WithFileNameExceedingMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var longFileName = new string('A', 256); // Exceeds 255 character limit
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: longFileName
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.FileName);
    }

    [Fact]
    public void Validate_WithValidFileName_ShouldNotHaveValidationError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "ValidFileName.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FileName);
    }

    [Fact]
    public void Validate_WithFileNameAtMaxLength_ShouldNotHaveValidationError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var maxLengthFileName = new string('A', 255); // Exactly at limit
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: maxLengthFileName
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.FileName);
    }

    #endregion

    #region User ID Validation Tests

    [Fact]
    public void Validate_WithEmptyUserId_ShouldHaveValidationError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.UserId)
            .WithErrorMessage("User Id is required");
    }

    [Fact]
    public void Validate_WithValidUserId_ShouldNotHaveValidationError()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "test.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.UserId);
    }

    #endregion

    #region Complete Validation Tests

    [Fact]
    public void Validate_WithAllValidFields_ShouldPassValidation()
    {
        // Arrange
        var mockFile = CreateMockFile("test.pdf", "Valid content");
        var command = new UploadSupportingDocCommand(
            File: mockFile,
            FileName: "ValidDocument.pdf"
        )
        {
            UserId = Guid.NewGuid()
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeTrue();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Validate_WithMultipleInvalidFields_ShouldHaveMultipleValidationErrors()
    {
        // Arrange
        var command = new UploadSupportingDocCommand(
            File: null!,
            FileName: string.Empty
        )
        {
            UserId = Guid.Empty
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().HaveCountGreaterThan(1);
        result.ShouldHaveValidationErrorFor(x => x.File);
        result.ShouldHaveValidationErrorFor(x => x.FileName);
        result.ShouldHaveValidationErrorFor(x => x.UserId);
    }

    #endregion

    #region Helper Methods

    private static IFormFile CreateMockFile(string fileName, string content)
    {
        var bytes = System.Text.Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        var file = A.Fake<IFormFile>();

        A.CallTo(() => file.FileName).Returns(fileName);
        A.CallTo(() => file.Length).Returns(bytes.Length);
        A.CallTo(() => file.OpenReadStream()).Returns(stream);

        return file;
    }

    #endregion
}
