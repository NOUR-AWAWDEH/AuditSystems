using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.DepartmentServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.LocationServices;
using AuditSystem.Contract.Interfaces.ModelServices.OrganizationServices.SubDepartmentServices;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Reports.DriftReport;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AuditSystem.UnitTests.Services.ReportServices.Helpers;

public class OrganizationalDataFetcherTests
{
    private readonly ICompanyService _companyService;
    private readonly ILocationService _locationService;
    private readonly IDepartmentService _departmentService;
    private readonly ISubDepartmentService _subDepartmentService;
    private readonly ILogger<OrganizationalDataFetcher> _logger;
    private readonly OrganizationalDataFetcher _sut;

    public OrganizationalDataFetcherTests()
    {
        _companyService = A.Fake<ICompanyService>();
        _locationService = A.Fake<ILocationService>();
        _departmentService = A.Fake<IDepartmentService>();
        _subDepartmentService = A.Fake<ISubDepartmentService>();
        _logger = A.Fake<ILogger<OrganizationalDataFetcher>>();

        _sut = new OrganizationalDataFetcher(
            _companyService,
            _locationService,
            _departmentService,
            _subDepartmentService,
            _logger
        );
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnOrganizationalData_WhenAllServicesSucceed()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            UserId = Guid.NewGuid(),
            CompanyId = Guid.NewGuid(),
            LocationId = Guid.NewGuid(),
            DepartmentId = Guid.NewGuid(),
            SubDepartmentId = Guid.NewGuid()
        };

        var company = new CompanyModel { Id = request.CompanyId, Name = "Test Company" };
        var location = new LocationModel { Id = request.LocationId, Name = "Test Location", CompanyId = request.CompanyId };
        var department = new DepartmentModel { Id = request.DepartmentId, Name = "Test Dept", LocationId = request.LocationId };
        var subDepartment = new SubDepartmentModel { Id = request.SubDepartmentId, Name = "Test SubDept", DepartmentId = request.DepartmentId };

        A.CallTo(() => _companyService.GetCompanyByIdAsync(request.CompanyId))
            .Returns(company);
        A.CallTo(() => _locationService.GetLocationByIdAsync(request.LocationId, request.UserId))
            .Returns(location);
        A.CallTo(() => _departmentService.GetDepartmentByIdAsync(request.DepartmentId, request.UserId))
            .Returns(department);
        A.CallTo(() => _subDepartmentService.GetSubDepartmentByIdAsync(request.SubDepartmentId, request.UserId))
            .Returns(subDepartment);

        // Act
        var result = await _sut.FetchAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Company.Should().Be(company);
        result.Location.Should().Be(location);
        result.Department.Should().Be(department);
        result.SubDepartment.Should().Be(subDepartment);

        A.CallTo(() => _companyService.GetCompanyByIdAsync(request.CompanyId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _locationService.GetLocationByIdAsync(request.LocationId, request.UserId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _departmentService.GetDepartmentByIdAsync(request.DepartmentId, request.UserId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _subDepartmentService.GetSubDepartmentByIdAsync(request.SubDepartmentId, request.UserId)).MustHaveHappenedOnceExactly();
    }
}