using AuditSystem.BusinessLogic.Helpers.UserHelpers.AppUserHelpers;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Domain.Entities.Users;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace AuditSystem.UnitTests.Helpers.UserHelpers.AppUserHelpers;

public class AppUserValidationHelperTests
{
    private readonly IRepository<Guid, AppUser> _mockRepo;
    private readonly UserManager<ApplicationUser> _mockUserManager;
    private readonly AppUserValidationHelper _helper;

    public AppUserValidationHelperTests()
    {
        _mockRepo = A.Fake<IRepository<Guid, AppUser>>();


        // UserManager requires a lot of dependencies in its constructor.
        // We can pass fakes/nulls for them using WithArgumentsForConstructor.
        _mockUserManager = A.Fake<UserManager<ApplicationUser>>(options => options.WithArgumentsForConstructor(new object?[] {
            A.Fake<IUserStore<ApplicationUser>>(),
            A.Fake<IOptions<IdentityOptions>>(),
            A.Fake<IPasswordHasher<ApplicationUser>>(),
            new List<IUserValidator<ApplicationUser>>(),
            new List<IPasswordValidator<ApplicationUser>>(),
            A.Fake<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            A.Fake<IServiceProvider>(),
            A.Fake<ILogger<UserManager<ApplicationUser>>>()
        }));

        _helper = new AppUserValidationHelper(_mockRepo, _mockUserManager);
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

        A.CallTo(() => _mockRepo.GetByIdAsync(userId))
            .Returns((AppUser?)null);

        var act = async () => await _helper.ValidateAppUserExists(userId);

        await act.Should()
            .ThrowAsync<KeyNotFoundException>()
            .WithMessage("*User not found*");
    }


    [Fact]
    public async Task ValidateAppUserExists_ShouldNotThrow_WhenUserExists()
    {
        var user = A.Fake<AppUser>();
        var userId = Guid.NewGuid();
        
        A.CallTo(() => _mockRepo.GetByIdAsync(userId))
            .Returns(user);

        var act = async () => await _helper.ValidateAppUserExists(userId);

        await act.Should().NotThrowAsync();
    }
}