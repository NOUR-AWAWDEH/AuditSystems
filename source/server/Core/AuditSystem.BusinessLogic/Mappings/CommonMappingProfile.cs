using AuditSystem.Application.Features.Common.Rating.Create;
using AuditSystem.Application.Features.Users.Create;
using AuditSystem.Application.Features.Users.Update;
using AuditSystem.Contract.Dtos.Register;
using AuditSystem.Contract.Models.Common;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Common;
using AuditSystem.Domain.Entities.Users;
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

        CreateMap<AppUser, UserModel>();
        CreateMap<UpdateUserCommand, UserModel>();
        CreateMap<CreateUserCommand, UserModel>();
        CreateMap<RegisterDto, CreateUserCommand>().ReverseMap();
    }
}
