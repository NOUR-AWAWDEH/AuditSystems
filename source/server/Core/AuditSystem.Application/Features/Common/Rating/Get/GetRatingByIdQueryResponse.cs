using AuditSystem.Contract.Models.Common;

namespace AuditSystem.Application.Features.Common.Rating.Get;

public sealed record class GetRatingByIdQueryResponse(RateModel RatingModel);