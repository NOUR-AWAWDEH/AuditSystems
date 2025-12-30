using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;
using AuditSystem.Contract.Interfaces.ModelServices.AuditServices.AuditProgramServices;
using AuditSystem.Contract.Interfaces.ModelServices.ObservationServices;
using AuditSystem.Contract.Interfaces.ModelServices.RiskControlsServices.ObjectiveServices;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using AuditSystem.Contract.Models.RiskControls;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Contract.Models.Pagination;

namespace AuditSystem.UnitTests.Services.ReportServices.Helpers;

public class ControlDataFetcherTests
{
    private readonly IObjectiveService _objectiveService;
    private readonly IRiskService _riskService;
    private readonly IAuditProgramService _auditProgramService;
    private readonly IObservationService _observationService;
    private readonly ILogger<ControlDataFetcher> _logger;

    private readonly ControlDataFetcher _sut;

    public ControlDataFetcherTests()
    {
        _objectiveService = A.Fake<IObjectiveService>();
        _riskService = A.Fake<IRiskService>();
        _auditProgramService = A.Fake<IAuditProgramService>();
        _observationService = A.Fake<IObservationService>();
        _logger = A.Fake<ILogger<ControlDataFetcher>>();

        _sut = new ControlDataFetcher(
            _objectiveService,
            _riskService,
            _auditProgramService,
            _observationService,
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

        var risksPage = new PaginationResult<RiskModel>
        {
            PageNumber = 1,
            PageSize = 10,
            Items = new List<RiskModel>
            {
                new RiskModel
                {
                    Id = Guid.NewGuid(),
                    ObjectiveId = objectiveId,
                    RiskName = "Test Risk"
                }
            },

            TotalCount = 1
        };



        var auditProgram = new AuditProgramModel
        {
            Id = auditProgramId,
            ExitControlId = Guid.NewGuid(),
            Description = "Test Audit Program",
            ActionOwnerId = Guid.NewGuid()
        };

        var observationsPage = new PaginationResult<ObservationModel>
        {
            PageNumber = 1,
            PageSize = 10,
            Items = new List<ObservationModel>
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
            },

            TotalCount = 1
        };


        A.CallTo(() =>
                _objectiveService.GetObjectiveByIdAsync(objectiveId, userId))
            .Returns(Task.FromResult(objective));

        A.CallTo(() =>
                _riskService.SearchByUserIdAsync(userId, "", objectiveId, A<Page?>._))
            .Returns(Task.FromResult(risksPage));


        A.CallTo(() =>
                _auditProgramService.GetAuditProgramByIdAsync(auditProgramId, userId))
            .Returns(Task.FromResult(auditProgram));

        A.CallTo(() =>
                _observationService.SearchByUserIdAsync(userId, "", auditProgramId, A<Page?>._))
            .Returns(Task.FromResult(observationsPage));

        // Act
        var result = await _sut.FetchAsync(request);

        // Assert
        result.Should().NotBeNull();

        result.Objective.Should().BeSameAs(objective);
        result.Risks.Should().BeSameAs(risksPage);
        result.AuditProgram.Should().BeSameAs(auditProgram);
        result.Observations.Should().BeSameAs(observationsPage);

        A.CallTo(() =>
                _objectiveService.GetObjectiveByIdAsync(objectiveId, userId))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _riskService.SearchByUserIdAsync(userId, "", objectiveId, null))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _auditProgramService.GetAuditProgramByIdAsync(auditProgramId, userId))
            .MustHaveHappenedOnceExactly();

        A.CallTo(() =>
                _observationService.SearchByUserIdAsync(userId, "", auditProgramId, null))
            .MustHaveHappenedOnceExactly();
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

        var emptyRisks = new PaginationResult<RiskModel>
        {
            PageNumber = 1,
            PageSize = 10,
            Items = new List<RiskModel>(),
            TotalCount = 0
        };

        var emptyObservations = new PaginationResult<ObservationModel>
        {
            PageNumber = 1,
            PageSize = 10,
            Items = new List<ObservationModel>(),
            TotalCount = 0
        };

        var auditProgram = new AuditProgramModel
        {
            Id = auditProgramId,
            ExitControlId = Guid.NewGuid(),
            Description = "Empty audit",
            ActionOwnerId = Guid.NewGuid()
        };

        // Fake the service calls
        A.CallTo(() => _objectiveService.GetObjectiveByIdAsync(objectiveId, userId))
            .Returns(Task.FromResult(objective));

        A.CallTo(() => _riskService.SearchByUserIdAsync(userId, "", objectiveId, A<Page?>._))
            .Returns(Task.FromResult(emptyRisks));

        A.CallTo(() => _auditProgramService.GetAuditProgramByIdAsync(auditProgramId, userId))
            .Returns(Task.FromResult(auditProgram));

        A.CallTo(() => _observationService.SearchByUserIdAsync(userId, "", auditProgramId, A<Page?>._))
            .Returns(Task.FromResult(emptyObservations));

        // Act
        ControlData result = await _sut.FetchAsync(request);

        // Assert
        result.Should().NotBeNull();
        result.Objective.Should().BeSameAs(objective);
        result.Risks.Items.Should().BeEmpty();
        result.Observations.Items.Should().BeEmpty();
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