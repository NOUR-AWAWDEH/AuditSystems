using AuditSystem.BusinessLogic.Helpers.UserHelpers.AppUserHelpers;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace AuditSystem.UnitTests.Helpers.UserHelpers.AppUserHelpers;

public class AppUserValidationHelperTests
{
    private readonly Mock<IRepository<Guid, AppUser>> _mockRepo;
    private readonly Mock<UserManager<ApplicationUser>> _mockUserManager;
    private readonly AppUserValidationHelper _helper;

    public AppUserValidationHelperTests()
    {
        _mockRepo = new Mock<IRepository<Guid, AppUser>>();

        var store = new Mock<IUserStore<ApplicationUser>>();
        _mockUserManager = new Mock<UserManager<ApplicationUser>>(
            store.Object,
            new Mock<IOptions<IdentityOptions>>().Object,
            new Mock<IPasswordHasher<ApplicationUser>>().Object,
            new List<IUserValidator<ApplicationUser>>(),
            new List<IPasswordValidator<ApplicationUser>>(),
            new Mock<ILookupNormalizer>().Object,
            new IdentityErrorDescriber(),
            new Mock<IServiceProvider>().Object,
            new Mock<ILogger<UserManager<ApplicationUser>>>().Object
        );

        _helper = new AppUserValidationHelper(_mockRepo.Object, _mockUserManager.Object);
    }

    [Fact]
    public async Task ValidateRoleNotAdmin_ShouldThrowException_WhenRoleIsAdmin()
    {
        // Arrange
        var role = "Admin";

        // Act
        var act = async () => await _helper.ValidateRoleNotAdmin(role);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>()
            .WithMessage("*Admin spawning*");
    }

    [Fact]
    public async Task ValidateRoleNotAdmin_ShouldThrowException_WhenRoleIsAdmin_CaseInsensitive()
    {
        // Arrange
        var role = "Admin";

        // Act
        var act = async () => await _helper.ValidateRoleNotAdmin(role);

        // Assert
        await act.Should()
            .ThrowAsync<InvalidOperationException>();
    }

    [Fact]
    public async Task ValidateRoleNotAdmin_ShouldNotThrow_WhenRoleIsNotAdmin()
    {
        // Arrange
        var role = "User";

        // Act
        var act = async () => await _helper.ValidateRoleNotAdmin(role);

        // Assert
        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task ValidateAppUserExists_ShouldThrowException_WhenUserDoesNotExist()
    {
        var userId = Guid.NewGuid();

        _mockRepo
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync((AppUser?)null);

        var act = async () => await _helper.ValidateAppUserExists(userId);

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*User not found*");
    }


    [Fact]
    public async Task ValidateAppUserExists_ShouldNotThrow_WhenUserExists()
    {
        var userId = Guid.NewGuid();

        var user = new AppUser
        {
            Id = userId,
            FirstName = "Test",
            LastName = "User",
            Email = "test@test.com",
            Role = "User",
            IdentityUserId = Guid.NewGuid(),
            CompanyId = Guid.NewGuid()
        };

        _mockRepo
            .Setup(r => r.GetByIdAsync(userId))
            .ReturnsAsync(user);

        var act = async () => await _helper.ValidateAppUserExists(userId);

        await act.Should().NotThrowAsync();
    }

}