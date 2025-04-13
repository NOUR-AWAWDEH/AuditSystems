using AuditSystem.Application.Features.Organisation.Company.Create;
using AuditSystem.Application.Features.Organisation.Company.Update;
using AuditSystem.Application.Features.Organisation.Department.Create;
using AuditSystem.Application.Features.Organisation.Department.Update;
using AuditSystem.Application.Features.Organisation.LocationModle.Create;
using AuditSystem.Application.Features.Organisation.LocationModle.Update;
using AuditSystem.Application.Features.Organisation.SubDepartment.Create;
using AuditSystem.Application.Features.Organisation.SubDepartment.Update;
using AuditSystem.Application.Features.Organisation.SubLocation.Create;
using AuditSystem.Application.Features.Organisation.SubLocation.Update;
using AuditSystem.Contract.Models.Organisation;
using AuditSystem.Domain.Entities.Organisation;
using AutoMapper;

namespace AuditSystem.BusinessLogic.Mappings;

public sealed class OrganizationMappingProfile : Profile
{
    public OrganizationMappingProfile()
    {
        //Organization
        //Company
        CreateMap<CompanyModel, Company>().ReverseMap();
        CreateMap<CreateCompanyCommand, CompanyModel>();
        CreateMap<UpdateCompanyCommand, CompanyModel>();

        //Department
        CreateMap<DepartmentModel, Department>().ReverseMap();
        CreateMap<CreateDepartmentCommand, DepartmentModel>();
        CreateMap<UpdateDepartmentCommand, DepartmentModel>();

        //Location
        CreateMap<LocationModel, Location>().ReverseMap();
        CreateMap<CreateLocationCommand, LocationModel>();
        CreateMap<UpdateLocationCommand, LocationModel>();

        //SubDepartment
        CreateMap<SubDepartmentModel, SubDepartment>().ReverseMap();
        CreateMap<CreateSubDepartmentCommand, SubDepartmentModel>();
        CreateMap<UpdateSubDepartmentCommand, SubDepartmentModel>();

        //SubLocation
        CreateMap<SubLocationModel, SubLocation>().ReverseMap();
        CreateMap<CreateSubLocationCommand, SubLocationModel>();
        CreateMap<UpdateSubLocationCommand, SubLocationModel>();
    }
}
