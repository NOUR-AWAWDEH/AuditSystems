using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Interfaces.Repositories;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Reports;
using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace AuditSystem.UnitTests.Services.ReportServices.DriftReportServices;

public class DriftReportServiceTests
{
    private readonly IRepository<Guid, DriftReport> _repository;
    private readonly IOrganizationalDataFetcher _orgFetcher;
    private readonly IControlDataFetcher _controlFetcher;
    private readonly IRecommendationsFetcher _recommendationsFetcher;
    private readonly IMapper _mapper;
    private readonly ILogger<DriftReportService> _logger;
    private readonly DriftReportService _sut;

    public DriftReportServiceTests()
    {
        _repository = A.Fake<IRepository<Guid, DriftReport>>();
        _orgFetcher = A.Fake<IOrganizationalDataFetcher>();
        _controlFetcher = A.Fake<IControlDataFetcher>();
        _recommendationsFetcher = A.Fake<IRecommendationsFetcher>();
        _mapper = A.Fake<IMapper>();
        _logger = A.Fake<ILogger<DriftReportService>>();

        _sut = new DriftReportService(
            _repository,
            _orgFetcher,
            _controlFetcher,
            _recommendationsFetcher,
            _mapper,
            _logger
        );
    }

    [Fact]
    public async Task CreateDriftReportAsync_WhenCommandIsNull_ShouldThrowArgumentNullException()
    {
        // Act
        Func<Task> act = async () => await _sut.CreateDriftReportAsync(null!);

        // Assert
        await act.Should().ThrowAsync<ArgumentNullException>();
    }

    [Fact]
    public async Task CreateDriftReportAsync_WithValidCommand_ShouldPersistAndReturnReport()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            CompanyId = Guid.NewGuid(),
            LocationId = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            ObjectiveId = Guid.NewGuid(),
            AuditProgramId = Guid.NewGuid()
        };

        var driftReportEntity = new DriftReport
        {
            Id = Guid.NewGuid(),
            CompanyId = request.CompanyId,
            LocationId = request.LocationId
        };

        // Organizational data setup
        var orgData = new OrganizationalData
        (
            new CompanyModel { Id = request.CompanyId, Name = "Test Company" },
            new LocationModel { Id = request.LocationId, Name = "Test Location", CompanyId = request.CompanyId },
            new DepartmentModel { Id = Guid.NewGuid(), Name = "Test Dept", LocationId = request.LocationId },
            new SubDepartmentModel { Id = Guid.NewGuid(), Name = "Test SubDept", DepartmentId = Guid.NewGuid() }
        );

        // Control data setup
        var objective = new ObjectiveModel { Id = request.ObjectiveId, Description = "Obj", RiskControlMatrixId = Guid.NewGuid() };
        var risks = new List<RiskModel> { new RiskModel { Id = Guid.NewGuid(), ObjectiveId = request.ObjectiveId, RiskName = "Test Risk" } };
        var auditProgram = new AuditProgramModel { Id = request.AuditProgramId, Description = "AP", ExitControlId = Guid.NewGuid(), ActionOwnerId = Guid.NewGuid() };
        var observations = new List<ObservationModel>
        {
            new ObservationModel
            {
                Id = Guid.NewGuid(),
                AuditProgramId = request.AuditProgramId,
                Highlight = "Highlight",
                Background = "Background",
                IsActive = true,
                RateId = Guid.NewGuid()
            }
        };
        var controlData = new ControlData(objective, risks, auditProgram, observations);

        // Recommendations
        var recommendationsList = new List<RecommendationModel>
        {
            new RecommendationModel
            {
                Id = Guid.NewGuid(),
                Description = "Rec1",
                Title = "Title",
                ObservationId = observations[0].Id
            }
        };

        // Mocks
        A.CallTo(() => _mapper.Map<DriftReport>(request)).Returns(driftReportEntity);
        A.CallTo(() => _repository.CreateAsync(driftReportEntity)).Returns(driftReportEntity);
        
        A.CallTo(() => _orgFetcher.FetchAsync(request)).Returns(orgData);
        A.CallTo(() => _controlFetcher.FetchAsync(request)).Returns(controlData);
        A.CallTo(() => _recommendationsFetcher.FetchAsync(request, observations)).Returns(recommendationsList);

        // Map results to final model (Identity mapping for simplicity in test)
        A.CallTo(() => _mapper.Map<CompanyModel>(orgData.Company)).Returns(orgData.Company);
        A.CallTo(() => _mapper.Map<LocationModel>(orgData.Location)).Returns(orgData.Location);
        A.CallTo(() => _mapper.Map<DepartmentModel>(orgData.Department)).Returns(orgData.Department);
        A.CallTo(() => _mapper.Map<SubDepartmentModel>(orgData.SubDepartment)).Returns(orgData.SubDepartment);
        A.CallTo(() => _mapper.Map<ObjectiveModel>(objective)).Returns(objective);
        A.CallTo(() => _mapper.Map<AuditProgramModel>(auditProgram)).Returns(auditProgram);
        A.CallTo(() => _mapper.Map<IReadOnlyList<RiskModel>>(risks)).Returns(risks);
        A.CallTo(() => _mapper.Map<IReadOnlyList<ObservationModel>>(observations)).Returns(observations);
        A.CallTo(() => _mapper.Map<IReadOnlyList<RecommendationModel>>(recommendationsList)).Returns(recommendationsList);

        // Act
        var result = await _sut.CreateDriftReportAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.UserId.Should().Be(request.UserId);
        result.Company.Should().Be(orgData.Company);
        result.Location.Should().Be(orgData.Location);
        result.Risks.Should().HaveCount(1);
        
        A.CallTo(() => _repository.CreateAsync(A<DriftReport>.That.Matches(e => e.CreatedByUserId == request.UserId))).MustHaveHappenedOnceExactly();
    }
}
