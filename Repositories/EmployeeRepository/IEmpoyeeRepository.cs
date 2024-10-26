public interface IEmployeeRepository
{
    PaginationResponse<IEnumerable<EmployeeReadDto>> GetAllEmployees(EmployeeFilter employeeFilter);
    EmployeeReadDto? GetEmployeeById(int id);
    bool CreateEmployee(EmployeeCreateDto employeeCreateDTO);
    bool DeleteEmployee(int id);
    bool UpdateEmployee(EmployeeUpdateDto employeeUpdateDTO);
    PaginationResponse<IEnumerable<EmployeeWithJournals?>> GetEmployeeWithJournals(BaseFilter filter);
    EmployeeWithJournals GetEmployeeWithJournalsByIdAndDate(int id, DateTime date);
}