using AuditSystem.Application.Features.Common.Rating.Create;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Domain.Entities.Common;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class CommonMappingProfile : Profile
{
    public CommonMappingProfile()
    {
        //Rating
        CreateMap<RatingModel, Rating>().ReverseMap();
        CreateMap<CreateRatingCommand, RatingModel>();
        CreateMap<RatingModel, Rating>().ReverseMap();
    }
}
