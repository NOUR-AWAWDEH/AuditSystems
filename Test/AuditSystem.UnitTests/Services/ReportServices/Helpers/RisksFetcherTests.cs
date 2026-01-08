using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;
using AuditSystem.Contract.Interfaces.ModelServices.RisksServices;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Reports.DriftReport;
using AuditSystem.Contract.Models.Risks;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;

namespace AuditSystem.UnitTests.Services.ReportServices.Helpers;

public class RisksFetcherTests
{
    private readonly IRiskService _riskService;
    private readonly ILogger<RisksFetcher> _logger;
    private readonly RisksFetcher _sut;

    public RisksFetcherTests()
    {
        _riskService = A.Fake<IRiskService>();
        _logger = A.Fake<ILogger<RisksFetcher>>();
        _sut = new RisksFetcher(_riskService, _logger);
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnRisks_WhenRisksExist()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            UserId = Guid.NewGuid(),
            ObjectiveId = Guid.NewGuid()
        };

        var observationId = Guid.NewGuid();
        var observationIds = new[] { observationId };

        var risks = new List<RiskModel>
        {
            new RiskModel { Id = Guid.NewGuid(), ObservationId = observationId, RiskName = "Risk 1" }
        };

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", observationId, A<Page>._))
            .Returns(Task.FromResult(new PaginationResult<RiskModel> 
            { 
                Items = risks,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 100
            }));

        // Act
        var result = await _sut.FetchAsync(request, observationIds);

        // Assert
        result.Should().BeEquivalentTo(risks);
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnEmptyList_WhenNoRisksFound()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            UserId = Guid.NewGuid(),
            ObjectiveId = Guid.NewGuid()
        };

        var observationId = Guid.NewGuid();
        var observationIds = new[] { observationId };

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", observationId, A<Page>._))
            .Returns(new PaginationResult<RiskModel> 
            { 
                Items = new List<RiskModel>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 100
            });

        // Act
        var result = await _sut.FetchAsync(request, observationIds);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnCombinedRisks_WhenMultipleObservationsProvided()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            UserId = Guid.NewGuid()
        };

        var obs1 = Guid.NewGuid();
        var obs2 = Guid.NewGuid();
        var observationIds = new[] { obs1, obs2 };

        var risks1 = new List<RiskModel> { new RiskModel { Id = Guid.NewGuid(), ObservationId = obs1, RiskName = "Risk 1" } };
        var risks2 = new List<RiskModel> { new RiskModel { Id = Guid.NewGuid(), ObservationId = obs2, RiskName = "Risk 2" } };

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", obs1, A<Page>._))
            .Returns(Task.FromResult(new PaginationResult<RiskModel> 
            { 
                Items = risks1,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 100
            }));

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", obs2, A<Page>._))
            .Returns(Task.FromResult(new PaginationResult<RiskModel> 
            { 
                Items = risks2,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 100
            }));

        // Act
        var result = await _sut.FetchAsync(request, observationIds);

        // Assert
        result.Should().HaveCount(2);
        result.Should().ContainEquivalentOf(risks1[0]);
        result.Should().ContainEquivalentOf(risks2[0]);
    }
}
