public record EmployeeFilter : BaseFilter
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}