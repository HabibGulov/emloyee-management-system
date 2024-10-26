public record JournalFilter:BaseFilter
{
    public DateTime StartDate{get; set;}
    public DateTime EndDate{get; set;}
}