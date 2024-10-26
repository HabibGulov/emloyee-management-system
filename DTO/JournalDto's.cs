public class JournalCreateDto
{
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public decimal Salary { get; set; }
    public string Comment { get; set; } = null!;
    public int TardinessMinutes { get; set; }
    public int EmployeeId { get; set; }
}

public class JournalUpdateDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public decimal Salary { get; set; }
    public string Comment { get; set; } = null!;
    public int TardinessMinutes { get; set; }
    public int EmployeeId { get; set; }
}

public class JournalReadDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public decimal Salary { get; set; }
    public string Comment { get; set; } = null!;
    public int TardinessMinutes { get; set; }
    public int EmployeeId { get; set; }
}