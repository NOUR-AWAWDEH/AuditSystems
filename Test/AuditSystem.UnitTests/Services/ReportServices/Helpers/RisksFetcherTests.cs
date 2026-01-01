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

        var risks = new List<RiskModel>
        {
            new RiskModel { Id = Guid.NewGuid(), ObjectiveId = request.ObjectiveId, RiskName = "Risk 1" }
        };

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", request.ObjectiveId, null))
            .Returns(new PaginationResult<RiskModel> 
            { 
                Items = risks,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            });

        // Act
        var result = await _sut.FetchAsync(request);

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

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", request.ObjectiveId, null))
            .Returns(new PaginationResult<RiskModel> 
            { 
                Items = new List<RiskModel>(),
                TotalCount = 0,
                PageNumber = 1,
                PageSize = 10
            });

        // Act
        var result = await _sut.FetchAsync(request);

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnEmptyList_WhenResultIsNull()
    {
        // Arrange
        var request = new GenerateDriftReportRequest
        {
            UserId = Guid.NewGuid(),
            ObjectiveId = Guid.NewGuid()
        };

        A.CallTo(() => _riskService.SearchByUserIdAsync(request.UserId, "", request.ObjectiveId, null))
            .Returns((PaginationResult<RiskModel>)null!);

        // Act
        var result = await _sut.FetchAsync(request);

        // Assert
        result.Should().BeEmpty();
    }
}
