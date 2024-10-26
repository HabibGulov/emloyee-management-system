public class Journal : BaseEntity
{
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public decimal Salary { get; set; }
    public string Comment { get; set; } = null!;
    public int TardinessMinutes { get; set; }
    public int EmployeeId { get; set; }

    public Employee employee { get; set; } = null!;
}