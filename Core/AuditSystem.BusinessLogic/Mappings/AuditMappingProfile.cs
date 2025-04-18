using AuditSystem.Application.Features.Audit.AuditDomain.Create;
using AuditSystem.Application.Features.Audit.AuditDomain.Update;
using AuditSystem.Application.Features.Audit.AuditEngagement.Create;
using AuditSystem.Application.Features.Audit.AuditEngagement.Update;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Create;
using AuditSystem.Application.Features.Audit.AuditPlanSummary.Update;
using AuditSystem.Application.Features.Audit.AuditUniverse.Create;
using AuditSystem.Application.Features.Audit.AuditUniverse.Update;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Create;
using AuditSystem.Application.Features.Audit.AuditUniverseObjective.Update;
using AuditSystem.Application.Features.Audit.BusinessObjective.Create;
using AuditSystem.Application.Features.Audit.BusinessObjective.Update;
using AuditSystem.Application.Features.Audit.Objective.Create;
using AuditSystem.Application.Features.Audit.Objective.Update;
using AuditSystem.Application.Features.Audit.SpecialProject.Create;
using AuditSystem.Application.Features.Audit.SpecialProject.Update;
using AuditSystem.Contract.Models.Audit;
using AuditSystem.Domain.Entities;
using AuditSystem.Domain.Entities.Audit;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class AuditMappingProfile : Profile
{
    public AuditMappingProfile()
    {

        //Domain
        CreateMap<AuditDomainModel, AuditDomain>().ReverseMap();
        CreateMap<CreateAuditDomainCommand, AuditDomainModel>();
        CreateMap<UpdateAuditDomainCommand, AuditDomainModel>();

        // AuditEngagement
        CreateMap<AuditEngagementModel, AuditEngagement>().ReverseMap();
        CreateMap<CreateAuditEngagementCommand, AuditEngagementModel>();
        CreateMap<UpdateAuditEngagementCommand, AuditEngagementModel>();

        //AuditPlanSummary
        CreateMap<AuditPlanSummaryModel, AuditPlanSummary>().ReverseMap();
        CreateMap<CreateAuditPlanSummaryCommand, AuditPlanSummaryModel>();
        CreateMap<UpdateAuditPlanSummaryCommand, AuditPlanSummaryModel>();

        //AuditUniverse
        CreateMap<AuditUniverseModel, AuditUniverse>().ReverseMap();
        CreateMap<CreateAuditUniverseCommand, AuditUniverseModel>();
        CreateMap<UpdateAuditUniverseCommand, AuditUniverseModel>();

        //AuditUniverseObjective
        CreateMap<AuditUniverseObjectiveModel, AuditUniverseObjective>().ReverseMap();
        CreateMap<CreateAuditUniverseObjectiveCommand, AuditUniverseObjectiveModel>();
        CreateMap<UpdateAuditUniverseObjectiveCommand, AuditUniverseObjectiveModel>();

        //BusinessObjective
        CreateMap<BusinessObjectiveModel, BusinessObjective>().ReverseMap();
        CreateMap<CreateBusinessObjectiveCommand, BusinessObjectiveModel>();
        CreateMap<UpdateBusinessObjectiveCommand, BusinessObjectiveModel>();

        //Objective
        CreateMap<ObjectiveModel, Objective>().ReverseMap();
        CreateMap<CreateObjectiveCommand, ObjectiveModel>();
        CreateMap<UpdateObjectiveCommand, ObjectiveModel>();

        //SpecialProject
        CreateMap<SpecialProjectModel, SpecialProject>().ReverseMap();
        CreateMap<CreateSpecialProjectCommand, SpecialProjectModel>();
        CreateMap<UpdateSpecialProjectCommand, SpecialProjectModel>();
    }
}
