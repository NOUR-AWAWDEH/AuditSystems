using AuditSystem.BusinessLogic.Services.EntityServices.ReportServices.DriftReportServices.Helpers;
using AuditSystem.Contract.Interfaces.ModelServices.RecommendationServices;
using AuditSystem.Contract.Models.Observation;
using AuditSystem.Contract.Models.Pagination;
using AuditSystem.Contract.Models.Recommendation;
using AuditSystem.Contract.Models.Reports.DriftReport;
using FakeItEasy;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace AuditSystem.UnitTests.Services.ReportServices.Helpers;

public class RecommendationsFetcherTests
{
    private readonly IRecommendationService _recommendationService;
    private readonly ILogger<RecommendationsFetcher> _logger;
    private readonly RecommendationsFetcher _sut;

    public RecommendationsFetcherTests()
    {
        _recommendationService = A.Fake<IRecommendationService>();
        _logger = A.Fake<ILogger<RecommendationsFetcher>>();

        _sut = new RecommendationsFetcher(_recommendationService, _logger);
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnRecommendations_WhenObservationsAreProvided()
    {
        // Arrange
        var request = new GenerateDriftReportRequest { UserId = Guid.NewGuid() };
        var observations = new List<ObservationModel>
        {
            new ObservationModel 
            { 
                Id = Guid.NewGuid(), 
                Highlight = "H1", 
                Background = "B1", 
                IsActive = true, 
                AuditProgramId = Guid.NewGuid(), 
                RateId = Guid.NewGuid() 
            },
            new ObservationModel 
            { 
                Id = Guid.NewGuid(), 
                Highlight = "H2", 
                Background = "B2", 
                IsActive = true, 
                AuditProgramId = Guid.NewGuid(), 
                RateId = Guid.NewGuid() 
            }
        };

        var recommendations1 = new List<RecommendationModel>
        {
            new RecommendationModel 
            { 
                Id = Guid.NewGuid(), 
                ObservationId = observations[0].Id,
                Title = "T1",
                Description = "D1"
            }
        };
        var recommendations2 = new List<RecommendationModel>
        {
            new RecommendationModel 
            { 
                Id = Guid.NewGuid(), 
                ObservationId = observations[1].Id,
                Title = "T2",
                Description = "D2"
            },
            new RecommendationModel 
            { 
                Id = Guid.NewGuid(), 
                ObservationId = observations[1].Id,
                Title = "T3",
                Description = "D3"
            }
        };

        A.CallTo(() => _recommendationService.SearchByUserIdAsync(request.UserId, "", observations[0].Id, null))
            .Returns(new PaginationResult<RecommendationModel>
            {
                Items = recommendations1,
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            });

        A.CallTo(() => _recommendationService.SearchByUserIdAsync(request.UserId, "", observations[1].Id, null))
            .Returns(new PaginationResult<RecommendationModel>
            {
                Items = recommendations2,
                TotalCount = 2,
                PageNumber = 1,
                PageSize = 10
            });

        // Act
        var result = await _sut.FetchAsync(request, observations);

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain(recommendations1);
        result.Should().Contain(recommendations2);

        foreach (var observation in observations)
        {
            A.CallTo(() => _recommendationService.SearchByUserIdAsync(request.UserId, "", observation.Id, null))
                .MustHaveHappenedOnceExactly();
        }
    }

    [Fact]
    public async Task FetchAsync_ShouldReturnEmptyList_WhenNoObservationsAreProvided()
    {
        // Arrange
        var request = new GenerateDriftReportRequest { UserId = Guid.NewGuid() };
        var observations = new List<ObservationModel>();

        // Act
        var result = await _sut.FetchAsync(request, observations);

        // Assert
        result.Should().BeEmpty();
        A.CallTo(() => _recommendationService.SearchByUserIdAsync(A<Guid>._, A<string>._, A<Guid>._, A<Page>._))
            .MustNotHaveHappened();
    }
}