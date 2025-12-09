using Ardalis.Result;
using AuditSystem.Application.Features.Users.Get.GetUserById;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Models.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AuditSystem.UnitTests.Features.Users.Get.GetUserById;

public class GetUserByIdQueryHandlerTests
{
    private readonly Mock<IAppUserService> _mockUserService;
    private readonly Mock<ILogger<GetUserByIdQueryHandler>> _mockLogger;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _mockUserService = new Mock<IAppUserService>();
        _mockLogger = new Mock<ILogger<GetUserByIdQueryHandler>>();
        _handler = new GetUserByIdQueryHandler(_mockUserService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var expectedUser = new UserModel(userId, "testuser", "Test", "User", "test@example.com", "Admin", null, null);

        var query = new GetUserByIdQuery(userId) { UserId = adminId };

        _mockUserService.Setup(x => x.GetUserByIdAsync(userId, adminId))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.User.Should().BeEquivalentTo(expectedUser);

        _mockUserService.Verify(x => x.GetUserByIdAsync(userId, adminId), Times.Once);
    }
}
