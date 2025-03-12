using AutoMapper;
using AuditSystem.Contract.Models.Risks;
using AuditSystem.Domain.Entities.Risks;

namespace IdentityTenantManagement.BusinessLogic.Mappings;

public sealed class EntitiesMappingProfile : Profile
{
    public EntitiesMappingProfile()
    {
        //Please add .ReverseMap() in case you want map in both directions
        CreateMap<RiskModel, Risk>().ReverseMap();
    }
}