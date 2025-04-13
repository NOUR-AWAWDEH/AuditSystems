using AuditSystem.Application.Features.UserDesignation.Create;
using AuditSystem.Application.Features.UserDesignation.Update;
using AuditSystem.Contract.Models.Users;
using AuditSystem.Domain.Entities.Users;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class UserDesignationMappingProfile : Profile
{
    public UserDesignationMappingProfile()
    {
        //UserDesignation
        CreateMap<UserDesignationModel, UserDesignation>().ReverseMap();
        CreateMap<CreateUserDesignationCommand, UserDesignationModel>();
        CreateMap<UpdateUserDesignationCommand, UserDesignationModel>();
    }
}
