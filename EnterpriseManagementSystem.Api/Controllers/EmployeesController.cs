using EnterpriseManagementSystem.Api.Models;
using EnterpriseManagementSystem.Application.Common;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Dtos;
using EnterpriseManagementSystem.Application.Features.EmployeeFeature.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseManagementSystem.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmployeesController : BaseController
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }
    [HttpGet]
    public async Task<IActionResult> GetAll(PagingSearchDto employeeSearchDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _employeeService.GetAllAsync(employeeSearchDto, cancellationToken);
            return Ok(ApiResponse.Success<PagedList<EmployeeResponseDto>>(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Failure<string>(ex.Message));
        }
    }


    [HttpGet("search")]
    public async Task<IActionResult> Search( PagingSearchDto employeeSearchDto, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _employeeService.SearchAsync(employeeSearchDto, cancellationToken);
            return  Ok(ApiResponse.Success<PagedList<EmployeeResponseDto>>(result));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Failure<string>(ex.Message));
        }
    }

    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var employee = await _employeeService.GetByIdAsync(id, cancellationToken);

            if (employee == null)
            {
                return NotFound(ApiResponse.Failure<string>($"Employee with ID {id} not found" ));
            }

            return Ok(ApiResponse.Success<EmployeeDto>(employee));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Failure<string>(ex.Message));
        }
    }


    [HttpPost]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var employee = await _employeeService.CreateAsync(dto, cancellationToken);
            return CreatedAtAction(nameof(GetById), ApiResponse.Success<int>(employee.Id) );
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Failure<string>(ex.Message));
        }
    }

   
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(EmployeeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateEmployeeDto dto, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != dto.EmployeeID)
            {
                return BadRequest(ApiResponse.Failure<string>("ID in URL does not match ID in request body" ));
            }

            var employee = await _employeeService.UpdateAsync(dto, cancellationToken);
            return Ok(ApiResponse.Success<EmployeeDto>(employee));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse.Failure<string>(ex.Message ));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Failure<string>(ex.Message ));
        }
    }

  
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _employeeService.DeleteAsync(id, cancellationToken);

            if (!result)
            {
                return NotFound(ApiResponse.Failure<string>($"Employee with ID {id} not found"));
            }

            return Ok(ApiResponse.Success<string>("Employee deleted successfully" ));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse.Failure<string>(ex.Message ));
        }
    }

   
    [HttpHead("{id}")]
    [HttpGet("{id}/exists")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Exists(int id, CancellationToken cancellationToken = default)
    {
        var exists = await _employeeService.ExistsAsync(id, cancellationToken);

        if (!exists)
        {
            return NotFound(ApiResponse.Failure<string>("Not Found"));
        }

        return Ok(ApiResponse.Success<bool>(true ));
    }
}
