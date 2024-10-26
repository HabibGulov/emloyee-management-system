public class EmployeeRepository(ManagementDbContext context) : IEmployeeRepository
{
    public PaginationResponse<IEnumerable<EmployeeReadDto>> GetAllEmployees(EmployeeFilter employeeFilter)
    {
        try
        {
            IQueryable<EmployeeReadDto> employees = context.Employees
                .Where(x => x.IsDeleted == false)
                .Select(x => x.EmployeeToEmployeeRead());

            if (employeeFilter.FirstName != null || employeeFilter.LastName != null)
                employees = employees.Where(x => x.FirstName.ToLower().Contains(employeeFilter.FirstName!.ToLower()) && x.LastName.ToLower().Contains(employeeFilter.LastName!.ToLower()));

            employees = employees
                .Skip((employeeFilter.PageNumber - 1) * employeeFilter.PageSize)
                .Take(employeeFilter.PageSize);

            int totalRecords = context.Employees.Where(x => x.IsDeleted == false).Count();

            return PaginationResponse<IEnumerable<EmployeeReadDto>>.Create(
                employeeFilter.PageNumber,
                employeeFilter.PageSize,
                totalRecords,
                employees);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return PaginationResponse<IEnumerable<EmployeeReadDto>>.Create(
                pageNumber: employeeFilter.PageNumber,
                pageSize: employeeFilter.PageSize,
                totalRecords: 0,
                data: Enumerable.Empty<EmployeeReadDto>()
            );
        }
    }

    public EmployeeReadDto? GetEmployeeById(int id)
    {
        try
        {
            Employee? employee = context.Employees.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return employee?.EmployeeToEmployeeRead();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool CreateEmployee(EmployeeCreateDto employeeCreateDTO)
    {
        try
        {
            bool isExisted = context.Employees
                .Any(x => x.FirstName.ToLower() == employeeCreateDTO.FirstName.ToLower()
                       && x.LastName.ToLower() == employeeCreateDTO.LastName.ToLower()
                       && x.IsDeleted == false);
            if (isExisted) return false;

            context.Employees.Add(employeeCreateDTO.EmployeeCreateToEmployee(context));
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool DeleteEmployee(int id)
    {
        try
        {
            Employee? employee = context.Employees.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (employee == null) return false;

            employee.DeleteEmployee();
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateEmployee(EmployeeUpdateDto employeeUpdateDTO)
    {
        try
        {
            Employee? employee = context.Employees.FirstOrDefault(x => x.Id == employeeUpdateDTO.Id && x.IsDeleted == false);
            if (employee == null) return false;

            employee.EmployeeUpdate(employeeUpdateDTO);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }
    public PaginationResponse<IEnumerable<EmployeeWithJournals?>> GetEmployeeWithJournals(BaseFilter filter)
    {
        try
        {
            var employeeWithJournalsQuery = from employee in context.Employees
                                            where employee.IsDeleted == false
                                            join journal in context.Journals on employee.Id equals journal.EmployeeId
                                            where journal.IsDeleted == false
                                            group journal by employee into employeeJournals
                                            select new EmployeeWithJournals
                                            {
                                                EmployeeId = employeeJournals.Key.Id,
                                                FirstName = employeeJournals.Key.FirstName,
                                                LastName = employeeJournals.Key.LastName,
                                                Journals = employeeJournals.Select(x => x.JournalToJournalRead()).ToList(),
                                                TotalSum = employeeJournals.Where(x => x.IsPresent == true).Sum(x => x.Salary)
                                            };
            var employeeWithJournals = employeeWithJournalsQuery
                                        .Skip((filter.PageNumber - 1) * filter.PageSize)
                                        .Take(filter.PageSize)
                                        .ToList();

            var totalRecords = employeeWithJournalsQuery.Count();

            return PaginationResponse<IEnumerable<EmployeeWithJournals?>>.Create(
                filter.PageNumber,
                filter.PageSize,
                totalRecords,
                employeeWithJournals
            );
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return PaginationResponse<IEnumerable<EmployeeWithJournals?>>.Create(0, 0, 0, new List<EmployeeWithJournals?>());
        }
    }
    public EmployeeWithJournals GetEmployeeWithJournalsByIdAndDate(int id, DateTime date)
    {
        try
        {
            DateTime utcDate = date.ToUniversalTime();


            var startOfMonth = new DateTime(utcDate.Year, utcDate.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var endOfMonth = utcDate;

            
            var employee = context.Employees
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => x.EmployeeToEmployeeRead())
                .FirstOrDefault();

            if (employee == null)
                return new EmployeeWithJournals(); 

            var journals = context.Journals
                .Where(x => x.EmployeeId == employee.Id
                            && !x.IsDeleted
                            && x.Date >= startOfMonth
                            && x.Date <= endOfMonth)
                .Select(x => x.JournalToJournalRead())
                .ToList();

            var totalSum = context.Journals
                .Where(x => x.EmployeeId == employee.Id && !x.IsDeleted && x.Date <= utcDate && x.IsPresent==true)
                .Sum(x => x.Salary);

            return new EmployeeWithJournals
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Journals = journals,
                TotalSum = totalSum
            };
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return new EmployeeWithJournals();
        }
    }
}