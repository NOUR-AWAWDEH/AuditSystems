using AuditSystem.Application.Features.Skills.Skill.Create;
using AuditSystem.Application.Features.Skills.Skill.Update;
using AuditSystem.Application.Features.Skills.SkillCategory.Create;
using AuditSystem.Application.Features.Skills.SkillCategory.Update;
using AuditSystem.Application.Features.Skills.SkillSet.Create;
using AuditSystem.Application.Features.Skills.SkillSet.Update;
using AuditSystem.Contract.Models.Skills;
using AuditSystem.Domain.Entities.Skills;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class SkillsMappingProfile : Profile
{
    public SkillsMappingProfile()
    {
        //Skill
        CreateMap<SkillModel, Skill>().ReverseMap();
        CreateMap<CreateSkillCommand, SkillModel>();
        CreateMap<UpdateSkillCommand, SkillModel>();

        //SkillSet
        CreateMap<SkillSetModel, SkillSet>().ReverseMap();
        CreateMap<CreateSkillSetCommand, SkillSetModel>();
        CreateMap<UpdateSkillSetCommand, SkillSetModel>();

        //SkillCategory
        CreateMap<SkillCategoryModel, SkillCategory>().ReverseMap();
        CreateMap<CreateSkillCategoryCommand, SkillCategoryModel>();
        CreateMap<UpdateSkillCategoryCommand, SkillCategoryModel>();
    }
}
