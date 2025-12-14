using Ardalis.Result;
using AuditSystem.Application.Features.Users.Get.GetUserById;
using AuditSystem.Contract.Interfaces.ModelServices.UserServices.AppUserServices;
using AuditSystem.Contract.Models.Users;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using FakeItEasy;
using Xunit;

namespace AuditSystem.UnitTests.Features.Users.Get.GetUserById;

public class GetUserByIdQueryHandlerTests
{
    private readonly IAppUserService _mockUserService;
    private readonly ILogger<GetUserByIdQueryHandler> _mockLogger;
    private readonly GetUserByIdQueryHandler _handler;

    public GetUserByIdQueryHandlerTests()
    {
        _mockUserService = A.Fake<IAppUserService>();
        _mockLogger = A.Fake<ILogger<GetUserByIdQueryHandler>>();
        _handler = new GetUserByIdQueryHandler(_mockUserService, _mockLogger);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUserExists()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var adminId = Guid.NewGuid();
        var expectedUser = new UserModel(userId, "testuser", "Test", "User", "test@example.com", "Admin", null, null);

        var query = new GetUserByIdQuery(userId) { UserId = adminId };

        A.CallTo(() => _mockUserService.GetUserByIdAsync(userId, adminId))
            .Returns(expectedUser);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.User.Should().BeEquivalentTo(expectedUser);

        A.CallTo(() => _mockUserService.GetUserByIdAsync(userId, adminId)).MustHaveHappenedOnceExactly();
    }
}
