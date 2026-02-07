using EnterpriseManagementSystem.Application.Common;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Dtos;

namespace EnterpriseManagementSystem.Application.Features.EmployeeFeature.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedList<EmployeeResponseDto>> GetAllAsync(PagingSearchDto employeeSearchDto, CancellationToken cancellationToken = default);
    Task<PagedList<EmployeeResponseDto>> SearchAsync(PagingSearchDto employeeSearchDto, CancellationToken cancellationToken = default);
    Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default);
}
