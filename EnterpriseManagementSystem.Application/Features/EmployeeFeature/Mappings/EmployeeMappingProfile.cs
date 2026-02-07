using AutoMapper;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Dtos;
using EnterpriseManagementSystem.Domain.Entities;

namespace EnterpriseManagementSystem.Application.Features.Feature1.Mappings
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {

            CreateMap<Employee, EmployeeDto>().ReverseMap();

        }
    }
}
