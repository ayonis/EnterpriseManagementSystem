using AutoMapper;
using EnterpriseManagementSystem.Application.Common;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Dtos;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Filters;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Interfaces;
using EnterpriseManagementSystem.Application.Interfaces;
using EnterpriseManagementSystem.Domain.Entities;
using System.Linq.Expressions;

namespace EnterpriseManagementSystem.Application.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IRepository<Employee, int> _employeeRepository;
    private readonly IMapper _mapper;


    public EmployeeService(IUnitOfWork unitOfWork, IRepository<Employee, int> employeeRepository, IMapper mapper)
    {
        _employeeRepository = employeeRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;

    }

    public async Task<EmployeeDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(id, track: false, cancellationToken);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<PagedList<EmployeeResponseDto>> GetAllAsync(PagingSearchDto employeeSearchDto, CancellationToken cancellationToken = default)
    {
        var pagedEmployees = await _employeeRepository.GetFilteredPaginatedAsync(
                                                    predicates: null,
                                                    pageNumber: employeeSearchDto.PageNumber,
                                                    pageSize: employeeSearchDto.PageSize,
                                                    track: false,
                                                    orderBy: e => e.Id,
                                                    ascending: false
                                                );

        var employeeItems = _mapper.Map<List<EmployeeResponseDto>>(pagedEmployees.Items);

        return new PagedList<EmployeeResponseDto>(
            pagedEmployees.TotalCount,
            pagedEmployees.PageNumber,
            pagedEmployees.PageSize,
            employeeItems
        );


    }

    public async Task<PagedList<EmployeeResponseDto>> SearchAsync(PagingSearchDto employeeSearchDto,CancellationToken cancellationToken = default)
    {
        var predicates = EmployeeFilterBuilder.Build(employeeSearchDto);

        var pagedEmployees = await _employeeRepository.GetFilteredPaginatedAsync(
                                                    predicates: predicates,
                                                    pageNumber: employeeSearchDto.PageNumber,
                                                    pageSize: employeeSearchDto.PageSize,
                                                    track: false,
                                                    orderBy: e => e.Id,
                                                    ascending: false
                                                );

        var employeeItems = _mapper.Map<List<EmployeeResponseDto>>(pagedEmployees.Items);

        return new PagedList<EmployeeResponseDto>(
            pagedEmployees.TotalCount,
            pagedEmployees.PageNumber,
            pagedEmployees.PageSize,
            employeeItems
        );
    }

    public async Task<EmployeeDto> CreateAsync(CreateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        var employee = new Employee
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            Department = dto.Department,
            Position = dto.Position,
            HireDate = dto.HireDate,
            IdentityUserId = dto.IdentityUserId
        };

        var addedEntity = await _employeeRepository.AddAsync(employee, cancellationToken);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<EmployeeDto>(addedEntity);
    }

    public async Task<EmployeeDto> UpdateAsync(UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(dto.EmployeeID, track: true, cancellationToken);

        if (employee == null)
        {
            throw new KeyNotFoundException($"Employee with ID {dto.EmployeeID} not found");
        }

        employee.FirstName = dto.FirstName;
        employee.LastName = dto.LastName;
        employee.Email = dto.Email;
        employee.PhoneNumber = dto.PhoneNumber;
        employee.Department = dto.Department;
        employee.Position = dto.Position;
        employee.HireDate = dto.HireDate;
        employee.IdentityUserId = dto.IdentityUserId;

        _employeeRepository.Update(employee);
        await _unitOfWork.SaveAsync(cancellationToken);

        return _mapper.Map<EmployeeDto>(employee);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var employee = await _employeeRepository.GetByIdAsync(id, track: true, cancellationToken);

        if (employee == null)
        {
            return false;
        }

        _employeeRepository.Remove(employee);
        await _unitOfWork.SaveAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _employeeRepository.ExistsAsync(e => e.Id == id, cancellationToken);
    }


}
