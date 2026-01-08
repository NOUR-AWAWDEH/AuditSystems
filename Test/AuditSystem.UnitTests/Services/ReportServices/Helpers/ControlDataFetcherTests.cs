using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Interfaces.ModelServices.ReportsServices.DriftReportServices;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;
using Xunit;

namespace AuditSystem.UnitTests.Services.ReportServices.Helpers;

public class ControlDataFetcherTests
{
    private readonly IObjectiveService _objectiveService;
    private readonly IRisksFetcher _risksFetcher;
    private readonly IAuditProgramService _auditProgramService;
    private readonly IObservationsFetcher _observationsFetcher;
    private readonly ILogger<ControlDataFetcher> _logger;

    private readonly ControlDataFetcher _sut;

    public ControlDataFetcherTests()
    {
        _objectiveService = A.Fake<IObjectiveService>();
        _risksFetcher = A.Fake<IRisksFetcher>();
        _auditProgramService = A.Fake<IAuditProgramService>();
        _observationsFetcher = A.Fake<IObservationsFetcher>();
        _logger = A.Fake<ILogger<ControlDataFetcher>>();

        _sut = new ControlDataFetcher(
            _objectiveService,
            _risksFetcher,
            _auditProgramService,
            _observationsFetcher,
            _logger
        );
    }

    [Fact]
    public async Task FetchAsync_ShouldFetchAllControlData_AndReturnControlData()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var objectiveId = Guid.NewGuid();
        var auditProgramId = Guid.NewGuid();

        var request = new GenerateDriftReportRequest
        {
            UserId = userId,
            ObjectiveId = objectiveId,
            AuditProgramId = auditProgramId
        };

        var objective = new ObjectiveModel
        {
            Id = objectiveId,
            RiskControlMatrixId = Guid.NewGuid(),
            Description = "Test Description"
        };

        var risks = new List<RiskModel>
        {
            new RiskModel
            {
                Id = Guid.NewGuid(),
                ObservationId = Guid.NewGuid(),
                RiskName = "Test Risk"
            }
        };

        var auditProgram = new AuditProgramModel
        {
            Id = auditProgramId,
            ExitControlId = Guid.NewGuid(),
            Description = "Test Audit Program",
        };

        var observations = new List<ObservationModel>
        {
            new ObservationModel
            {
                Id = Guid.NewGuid(),
                AuditProgramId = auditProgramId,
                Highlight = "Test Highlight",
                Background = "Test Background",
                IsActive = true,
                RateId = Guid.NewGuid()
            }
        };

        A.CallTo(() => _objectiveService.GetObjectiveByIdAsync(objectiveId, userId))
            .Returns(objective);

        A.CallTo(() => _risksFetcher.FetchAsync(request, A<IEnumerable<Guid>>._))
            .Returns(risks);

        A.CallTo(() => _auditProgramService.GetAuditProgramByIdAsync(auditProgramId, userId))
            .Returns(auditProgram);

        A.CallTo(() => _observationsFetcher.FetchAsync(request))
            .Returns(observations);

        // Act
        var result = await _sut.FetchAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Objective.Should().Be(objective);
        result.Risks.Should().BeEquivalentTo(risks);
        result.AuditProgram.Should().Be(auditProgram);
        result.Observations.Should().BeEquivalentTo(observations);

        A.CallTo(() => _objectiveService.GetObjectiveByIdAsync(objectiveId, userId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _risksFetcher.FetchAsync(request, A<IEnumerable<Guid>>._)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _auditProgramService.GetAuditProgramByIdAsync(auditProgramId, userId)).MustHaveHappenedOnceExactly();
        A.CallTo(() => _observationsFetcher.FetchAsync(request)).MustHaveHappenedOnceExactly();
    }

    [Fact]
    public async Task FetchAsync_WithNoRisksOrObservations_ReturnsControlDataWithEmptyCollections()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var objectiveId = Guid.NewGuid();
        var auditProgramId = Guid.NewGuid();

        var request = new GenerateDriftReportRequest
        {
            UserId = userId,
            ObjectiveId = objectiveId,
            AuditProgramId = auditProgramId
        };

        var objective = new ObjectiveModel
        {
            Id = objectiveId,
            RiskControlMatrixId = Guid.NewGuid(),
            Description = "Empty test"
        };

        var auditProgram = new AuditProgramModel
        {
            Id = auditProgramId,
            ExitControlId = Guid.NewGuid(),
            Description = "Empty audit",
        };

        // Fake the service calls
        A.CallTo(() => _objectiveService.GetObjectiveByIdAsync(objectiveId, userId))
            .Returns(objective);

        A.CallTo(() => _risksFetcher.FetchAsync(request, A<IEnumerable<Guid>>._))
            .Returns(new List<RiskModel>());

        A.CallTo(() => _auditProgramService.GetAuditProgramByIdAsync(auditProgramId, userId))
            .Returns(auditProgram);

        A.CallTo(() => _observationsFetcher.FetchAsync(request))
            .Returns(new List<ObservationModel>());

        // Act
        ControlData result = await _sut.FetchAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Objective.Should().Be(objective);
        result.Risks.Should().BeEmpty();
        result.Observations.Should().BeEmpty();
    }


    [Fact]
    public async Task FetchAsync_WhenObjectiveServiceThrows_PropagatesException()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            UserId = Guid.NewGuid(),
            ObjectiveId = Guid.NewGuid(),
            AuditProgramId = Guid.NewGuid()
        };

        A.CallTo(() => _objectiveService.GetObjectiveByIdAsync(request.ObjectiveId, request.UserId))
            .ThrowsAsync(new InvalidOperationException("Objective not found"));

        // Act
        Func<Task> act = async () => await _sut.FetchAsync(request);

        // Assert
        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Objective not found");
    }

}