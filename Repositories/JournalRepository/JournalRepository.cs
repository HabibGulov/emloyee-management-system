public class JournalRepository(ManagementDbContext context) : IJournalRepository
{
    public PaginationResponse<IEnumerable<JournalReadDto>> GetAllJournals(JournalFilter journalFilter)
    {
        try
        {
            IQueryable<JournalReadDto> journals = context.Journals
                .Where(x => x.IsDeleted == false)
                .Select(x => x.JournalToJournalRead());

            if (journalFilter?.StartDate != null && journalFilter?.EndDate!=null)
                journals = journals.Where(x => x.Date>=journalFilter.StartDate && x.Date<journalFilter.EndDate);

            journals = journals
                .Skip((journalFilter!.PageNumber - 1) * journalFilter.PageSize)
                .Take(journalFilter.PageSize);

            int totalRecords = context.Journals.Where(x => x.IsDeleted == false).Count();

            return PaginationResponse<IEnumerable<JournalReadDto>>.Create(
                journalFilter.PageNumber,
                journalFilter.PageSize,
                totalRecords,
                journals);
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return  PaginationResponse<IEnumerable<JournalReadDto>>.Create(
                pageNumber: journalFilter.PageNumber,
                pageSize: journalFilter.PageSize,
                totalRecords: 0,
                data: Enumerable.Empty<JournalReadDto>()
            );

        }
    }

    public JournalReadDto? GetJournalById(int id)
    {
        try
        {
            Journal? journal = context.Journals.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            return journal?.JournalToJournalRead();
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return null;
        }
    }

    public bool CreateJournal(JournalCreateDto journalCreateDTO)
    {
        try
        {
            context.Journals.Add(journalCreateDTO.JournalCreateToJournal(context));
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool DeleteJournal(int id)
    {
        try
        {
            Journal? journal = context.Journals.FirstOrDefault(x => x.Id == id && x.IsDeleted == false);
            if (journal == null) return false;

            journal.DeleteJournal();
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }

    public bool UpdateJournal(JournalUpdateDto journalUpdateDTO)
    {
        try
        {
            Journal? journal = context.Journals.FirstOrDefault(x => x.Id == journalUpdateDTO.Id && x.IsDeleted == false);
            if (journal == null) return false;

            journal.JournalUpdate(journalUpdateDTO);
            context.SaveChanges();
            return true;
        }
        catch (Exception ex)
        {
            System.Console.WriteLine(ex.Message);
            return false;
        }
    }
}
