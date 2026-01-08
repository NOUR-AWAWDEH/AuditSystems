using AuditSystem.DataAccess.Contexts;
using AuditSystem.Domain.Entities.Organization;
using AuditSystem.Domain.Entities.Users;
using AuditSystem.Host.Seed.Services;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace AuditSystem.UnitTests.Infrastructure.Seed;

[Collection("Sequential")]
public class AdminSeedingOrchestratorTests : IDisposable
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<Role> _roleManager;
    private readonly ApplicationDbContext _dbContext;
    private readonly ILogger<AdminSeedingOrchestrator> _logger;
    private readonly AdminSeedingOrchestrator _sut;

    private readonly Guid _companyId = Guid.NewGuid();
    private readonly Guid _jobTitleId = Guid.NewGuid();

    public AdminSeedingOrchestratorTests()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _dbContext = new ApplicationDbContext(options);

        _userManager = A.Fake<UserManager<ApplicationUser>>(o => o.WithArgumentsForConstructor(new object?[] {
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

        _roleManager = A.Fake<RoleManager<Role>>(o => o.WithArgumentsForConstructor(new object?[] {
            A.Fake<IRoleStore<Role>>(),
            new List<IRoleValidator<Role>>(),
            A.Fake<ILookupNormalizer>(),
            new IdentityErrorDescriber(),
            A.Fake<ILogger<RoleManager<Role>>>()
        }));

        _logger = A.Fake<ILogger<AdminSeedingOrchestrator>>();

        _sut = new AdminSeedingOrchestrator(_userManager, _roleManager, _dbContext, _logger);

        // Common successful environment setup
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_EMAIL", "admin@test.com");
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_PASSWORD", "Admin@123");
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_USERNAME", "admin");
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_COMPANY_ID", _companyId.ToString());
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_JOBTITLE_ID", _jobTitleId.ToString());
    }

    public void Dispose()
    {
        // Cleanup environment variables
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_EMAIL", null);
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_PASSWORD", null);
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_USERNAME", null);
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_COMPANY_ID", null);
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_JOBTITLE_ID", null);
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_FIRSTNAME", null);
        Environment.SetEnvironmentVariable("ADMIN_INITIAL_LASTNAME", null);

        _dbContext.Dispose();
    }

    [Fact]
    public async Task SeedAsync_ShouldSeedAdmin_WhenEverythingIsCorrectAndUserMissing()
    {
        // Arrange
        await SeedRequiredDataAsync();

        A.CallTo(() => _userManager.FindByEmailAsync("admin@test.com"))
            .Returns((ApplicationUser?)null);
        A.CallTo(() => _userManager.FindByNameAsync("admin"))
            .Returns((ApplicationUser?)null);
        A.CallTo(() => _roleManager.RoleExistsAsync(A<string>._))
            .Returns(true);
        A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>._, "Admin@123"))
            .Returns(IdentityResult.Success);
        A.CallTo(() => _userManager.AddToRoleAsync(A<ApplicationUser>._, Role.Admin))
            .Returns(IdentityResult.Success);
        A.CallTo(() => _roleManager.FindByNameAsync(Role.Admin))
            .Returns(new Role { Id = Guid.NewGuid(), Name = Role.Admin });

        // Act
        await _sut.SeedAsync();

        // Assert
        _dbContext.AppUsers.Should().ContainSingle(u => u.Email == "admin@test.com");
        _dbContext.Set<CompanyUser>().Should().ContainSingle(cu => cu.CompanyId == _companyId);
    }

    [Fact]
    public async Task SeedAsync_ShouldThrowException_WhenCompanyMissing()
    {
        // Arrange - No company seeded

        // Act
        var act = async () => await _sut.SeedAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*Company not found*");
    }

    [Fact]
    public async Task SeedAsync_ShouldThrowException_WhenJobTitleMissing()
    {
        // Arrange
        _dbContext.Companies.Add(new Company { Id = _companyId, Name = "Test Co" });
        await _dbContext.SaveChangesAsync();

        // Act
        var act = async () => await _sut.SeedAsync();

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("*JobTitle not found*");
    }

    [Fact]
    public async Task SeedAsync_ShouldBeIdempotent_WhenUserAlreadyExists()
    {
        // Arrange
        await SeedRequiredDataAsync();
        var existingUser = new ApplicationUser { Id = Guid.NewGuid(), Email = "admin@test.com", UserName = "admin" };

        A.CallTo(() => _userManager.FindByEmailAsync("admin@test.com"))
            .Returns(existingUser);
        A.CallTo(() => _userManager.IsInRoleAsync(existingUser, Role.Admin))
            .Returns(true);
        A.CallTo(() => _roleManager.RoleExistsAsync(A<string>._))
            .Returns(true);
        
        // Ensure AppUser also exists to avoid creation
        _dbContext.AppUsers.Add(new AppUser { 
            Id = existingUser.Id, 
            IdentityUserId = existingUser.Id, 
            Email = existingUser.Email, 
            FirstName = "S", 
            LastName = "A", 
            UserName = existingUser.UserName,
            JobTitleId = _jobTitleId
        });
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.SeedAsync();

        // Assert
        A.CallTo(() => _userManager.CreateAsync(A<ApplicationUser>._, A<string>._)).MustNotHaveHappened();
        _dbContext.AppUsers.Should().HaveCount(1);
    }

    [Fact]
    public async Task SeedAsync_ShouldCreateRoles_WhenRolesMissing()
    {
        // Arrange
        await SeedRequiredDataAsync();
        A.CallTo(() => _roleManager.RoleExistsAsync(A<string>._)).Returns(false);
        A.CallTo(() => _roleManager.CreateAsync(A<Role>._)).Returns(IdentityResult.Success);
        
        // Mock user existing to skip user creation logic and focus on roles
        var existingUser = new ApplicationUser { Id = Guid.NewGuid(), Email = "admin@test.com" };
        A.CallTo(() => _userManager.FindByEmailAsync("admin@test.com")).Returns(existingUser);
        A.CallTo(() => _userManager.IsInRoleAsync(existingUser, Role.Admin)).Returns(true);
        _dbContext.AppUsers.Add(new AppUser { Id = existingUser.Id, IdentityUserId = existingUser.Id, Email = "admin@test.com", FirstName = "A", LastName = "B", UserName = "admin", JobTitleId = _jobTitleId });
        await _dbContext.SaveChangesAsync();

        // Act
        await _sut.SeedAsync();

        // Assert
        A.CallTo(() => _roleManager.CreateAsync(A<Role>.That.Matches(r => r.Name == Role.Admin))).MustHaveHappenedOnceExactly();
    }

    private async Task SeedRequiredDataAsync()
    {
        _dbContext.Companies.Add(new Company { Id = _companyId, Name = "Test Co" });
        _dbContext.Set<JobTitle>().Add(new JobTitle { Id = _jobTitleId, Name = "Test Title", CompanyId = _companyId });
        await _dbContext.SaveChangesAsync();
    }
}

[CollectionDefinition("Sequential", DisableParallelization = true)]
public class SequentialCollection : ICollectionFixture<object>
{
}
