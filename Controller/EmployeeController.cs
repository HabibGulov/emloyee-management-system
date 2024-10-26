using Microsoft.AspNetCore.Mvc;

namespace DTO_Pagination_Filtering_Mapping;

[ApiController]
[Route("/api/employees")]
public sealed class EmployeeController : ControllerBase
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeController(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetEmployees([FromQuery] EmployeeFilter filter)
    {
        var result = _employeeRepository.GetAllEmployees(filter);
        return Ok(ApiResponse<PaginationResponse<IEnumerable<EmployeeReadDto>>>.Success(null!, result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetEmployeeById(int id)
    {
        var employee = _employeeRepository.GetEmployeeById(id);
        return employee != null
            ? Ok(ApiResponse<EmployeeReadDto>.Success(null!, employee))
            : NotFound(ApiResponse<EmployeeReadDto>.Fail(null!, null));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateEmployee([FromBody] EmployeeCreateDto employeeCreateDto)
    {
        var result = _employeeRepository.CreateEmployee(employeeCreateDto);
        return result
            ? Ok(ApiResponse<bool>.Success(null!, result))
            : BadRequest(ApiResponse<bool>.Fail(null!, result));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateEmployee([FromBody] EmployeeUpdateDto employeeUpdateDto)
    {
        var result = _employeeRepository.UpdateEmployee(employeeUpdateDto);
        return result
            ? Ok(ApiResponse<bool>.Success(null!, result))
            : NotFound(ApiResponse<bool>.Fail(null!, result));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteEmployee(int id)
    {
        var result = _employeeRepository.DeleteEmployee(id);
        return result
            ? Ok(ApiResponse<bool>.Success(null!, result))
            : NotFound(ApiResponse<bool>.Fail(null!, result));
    }

    [HttpGet("/get-all-employees-and-journals")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetEmployeesWithJournals([FromQuery] BaseFilter filter)
    {
        var paginatedResult = _employeeRepository.GetEmployeeWithJournals(filter);

        return paginatedResult.Data != null
            ? Ok(ApiResponse<PaginationResponse<IEnumerable<EmployeeWithJournals>>>.Success(null!, paginatedResult!))
            : NotFound(ApiResponse<PaginationResponse<IEnumerable<EmployeeWithJournals>>>.Fail(null!, paginatedResult!));
    }

    [HttpGet("{id:int}/{date:datetime}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetEmployeeWithJournalsByIdAndDate(int id, DateTime date)
    {
        var result = _employeeRepository.GetEmployeeWithJournalsByIdAndDate(id, date);

        return result != null && result.Journals.Any()
            ? Ok(ApiResponse<EmployeeWithJournals>.Success(null!, result))
            : NotFound(ApiResponse<EmployeeWithJournals>.Fail(null!, result));
    }
}
