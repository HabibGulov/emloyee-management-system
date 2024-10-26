public class EmployeeWithJournals
{
    public int EmployeeId{get; set;}
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public IEnumerable<JournalReadDto> Journals { get; set; } = [];
    public decimal TotalSum { get; set; }
}