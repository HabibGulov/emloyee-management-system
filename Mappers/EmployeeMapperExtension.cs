public static class JournalMapperExtension
{
    public static EmployeeReadDto EmployeeToEmployeeRead(this Employee employee)
    {
        return new EmployeeReadDto()
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email
        };
    }

    public static Employee EmployeeUpdate(this Employee employee, EmployeeUpdateDto employeeUpdateDto)
    {
        employee.FirstName = employeeUpdateDto.FirstName;
        employee.LastName = employeeUpdateDto.LastName;
        employee.Email = employeeUpdateDto.Email;
        employee.UpdatedAt = DateTime.UtcNow;
        employee.Version += 1;
        return employee;
    }

    public static Employee EmployeeCreateToEmployee(this EmployeeCreateDto employeeCreateDto, ManagementDbContext bookDbContext)
    {
        int maxId = bookDbContext.Employees.Where(x => x.IsDeleted == false).Any() ? 
            bookDbContext.Employees.Where(x => x.IsDeleted == false).Max(x => x.Id) + 1 : 1;

        return new Employee()
        {
            Id = maxId,
            FirstName = employeeCreateDto.FirstName,
            LastName = employeeCreateDto.LastName,
            Email = employeeCreateDto.Email,
            CreatedAt = DateTime.UtcNow
        };
    }

    public static Employee DeleteEmployee(this Employee employee)
    {
        employee.IsDeleted = true;
        employee.DeletedAt = DateTime.UtcNow;
        employee.UpdatedAt = DateTime.UtcNow;
        employee.Version += 1;
        return employee;
    }
}