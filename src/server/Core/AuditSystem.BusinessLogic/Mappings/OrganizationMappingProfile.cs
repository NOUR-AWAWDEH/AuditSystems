using AuditSystem.Application.Features.Organization.Company.Create;
using AuditSystem.Application.Features.Organization.Company.Update;
using AuditSystem.Application.Features.Organization.Department.Create;
using AuditSystem.Application.Features.Organization.Department.Update;
using AuditSystem.Application.Features.Organization.Location.Create;
using AuditSystem.Application.Features.Organization.Location.Update;
using AuditSystem.Application.Features.Organization.SubDepartment.Create;
using AuditSystem.Application.Features.Organization.SubDepartment.Update;
using AuditSystem.Application.Features.Organization.SubLocation.Create;
using AuditSystem.Application.Features.Organization.SubLocation.Update;
using AuditSystem.Contract.Models.Organization;
using AuditSystem.Domain.Entities.Organization;
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
