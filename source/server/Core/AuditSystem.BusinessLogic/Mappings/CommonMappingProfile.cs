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
        CreateMap<RateModel, Rate>().ReverseMap();
        CreateMap<CreateRateCommand, RateModel>();
        CreateMap<RateModel, Rate>().ReverseMap();
    }
}
