using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace AuditSystem.UnitTests.Services.ReportServices;

public class DriftReportInfoServiceTests
{
    private readonly IOrganizationalDataFetcher _orgFetcher;
    private readonly IControlDataFetcher _controlFetcher;
    private readonly IRecommendationsFetcher _recommendationsFetcher;
    private readonly IMapper _mapper;
    private readonly ILogger<DriftReportInfoService> _logger;

    private readonly DriftReportInfoService _sut;

    public DriftReportInfoServiceTests()
    {
        _orgFetcher = A.Fake<IOrganizationalDataFetcher>();
        _controlFetcher = A.Fake<IControlDataFetcher>();
        _recommendationsFetcher = A.Fake<IRecommendationsFetcher>();
        _mapper = A.Fake<IMapper>();
        _logger = A.Fake<ILogger<DriftReportInfoService>>();

        _sut = new DriftReportInfoService(
            _orgFetcher,
            _controlFetcher,
            _recommendationsFetcher,
            _mapper,
            _logger
        );
    }

    [Fact]
    public async Task GenerateReportData_ShouldReturnCompleteReport()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            CompanyId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ObjectiveId = Guid.NewGuid(),
            AuditProgramId = Guid.NewGuid()
        };

        // Organizational data
        var orgData = new OrganizationalData
        (
            new CompanyModel { Id = Guid.NewGuid(), Name = "Test Company" },
            new LocationModel { Id = Guid.NewGuid(), Name = "Test Location", CompanyId = Guid.NewGuid() },
            new DepartmentModel { Id = Guid.NewGuid(), Name = "Test Dept", LocationId = Guid.NewGuid() },
            new SubDepartmentModel { Id = Guid.NewGuid(), Name = "Test SubDept", DepartmentId = Guid.NewGuid() }
        );

        // Control data
        var objective = new ObjectiveModel { Id = request.ObjectiveId, Description = "Obj", RiskControlMatrixId = Guid.NewGuid() };
        var risks = new List<RiskModel> { new RiskModel { Id = Guid.NewGuid(), ObjectiveId = request.ObjectiveId, RiskName = "Test Risk" } };
        var auditProgram = new AuditProgramModel { Id = request.AuditProgramId, Description = "AP", ActionOwnerId = Guid.NewGuid(), ExitControlId = Guid.NewGuid() };
        var observations = new List<ObservationModel>
        {
            new ObservationModel
            {
                Id = Guid.NewGuid(),
                AuditProgramId = request.AuditProgramId,
                Background = "background test",
                Highlight = "Highlight test",
                IsActive = true,
                RateId = Guid.NewGuid()
            }
        };
        var controlData = new ControlData(objective, risks, auditProgram, observations);

        // Recommendations
        var recommendationsList = new List<RecommendationModel>
        {
            new RecommendationModel { Id = Guid.NewGuid(), Description = "Rec1", ObservationId = Guid.NewGuid(), Title = "Title test" }
        };

        // Fake service calls
        A.CallTo(() => _orgFetcher.FetchAsync(request)).Returns(orgData);
        A.CallTo(() => _controlFetcher.FetchAsync(request)).Returns(controlData);
        A.CallTo(() => _recommendationsFetcher.FetchAsync(request, observations)).Returns(recommendationsList);

        // Identity mapping
        A.CallTo(() => _mapper.Map<CompanyModel>(A<CompanyModel>._)).ReturnsLazily(x => (CompanyModel)x.Arguments[0]!);
        A.CallTo(() => _mapper.Map<LocationModel>(A<LocationModel>._)).ReturnsLazily(x => (LocationModel)x.Arguments[0]!);
        A.CallTo(() => _mapper.Map<DepartmentModel>(A<DepartmentModel>._)).ReturnsLazily(x => (DepartmentModel)x.Arguments[0]!);
        A.CallTo(() => _mapper.Map<SubDepartmentModel>(A<SubDepartmentModel>._)).ReturnsLazily(x => (SubDepartmentModel)x.Arguments[0]!);
        A.CallTo(() => _mapper.Map<ObjectiveModel>(A<ObjectiveModel>._)).ReturnsLazily(x => (ObjectiveModel)x.Arguments[0]!);
        A.CallTo(() => _mapper.Map<AuditProgramModel>(A<AuditProgramModel>._)).ReturnsLazily(x => (AuditProgramModel)x.Arguments[0]!);

        // Act
        var result = await _sut.GenerateReportData(request);

        // Assert
        result.Should().NotBeNull();
        result.Company.Should().Be(orgData.Company);
        result.Location.Should().Be(orgData.Location);
        result.Department.Should().Be(orgData.Department);
        result.SubDepartment.Should().Be(orgData.SubDepartment);

        result.Objective.Should().Be(controlData.Objective);
        result.Risks.Should().BeEquivalentTo(controlData.Risks);
        result.AuditProgram.Should().Be(controlData.AuditProgram);
        result.Observations.Should().BeEquivalentTo(controlData.Observations);
        result.Recommendations.Should().BeEquivalentTo(recommendationsList);
    }
}