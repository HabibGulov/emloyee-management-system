public static class EmployeeMapperExtension
{
    public static JournalReadDto JournalToJournalRead(this Journal journal)
    {
        return new JournalReadDto()
        {
            Id = journal.Id,
            Date = journal.Date,
            IsPresent = journal.IsPresent,
            Salary = journal.Salary,
            Comment = journal.Comment,
            TardinessMinutes = journal.TardinessMinutes,
            EmployeeId = journal.EmployeeId
        };
    }

    // Метод для обновления Journal с помощью JournalUpdateDto
    public static Journal JournalUpdate(this Journal journal, JournalUpdateDto journalUpdateDto)
    {
        journal.Date = journalUpdateDto.Date;
        journal.IsPresent = journalUpdateDto.IsPresent;
        journal.Salary = journalUpdateDto.Salary;
        journal.Comment = journalUpdateDto.Comment;
        journal.TardinessMinutes = journalUpdateDto.TardinessMinutes;
        journal.UpdatedAt = DateTime.UtcNow;
        journal.Version += 1;
        return journal;
    }

    // Метод для создания нового Journal из JournalCreateDto
    public static Journal JournalCreateToJournal(this JournalCreateDto journalCreateDto, ManagementDbContext bookDbContext)
    {
        int maxId = bookDbContext.Journals.Where(x => x.IsDeleted == false).Any() ? 
            bookDbContext.Journals.Where(x => x.IsDeleted == false).Max(x => x.Id) + 1 : 1;

        return new Journal()
        {
            Id = maxId,
            Date = journalCreateDto.Date,
            IsPresent = journalCreateDto.IsPresent,
            Salary = journalCreateDto.Salary,
            Comment = journalCreateDto.Comment,
            TardinessMinutes = journalCreateDto.TardinessMinutes,
            EmployeeId = journalCreateDto.EmployeeId,
            CreatedAt = DateTime.UtcNow
        };
    }

    // Метод для "удаления" Journal
    public static Journal DeleteJournal(this Journal journal)
    {
        journal.IsDeleted = true;
        journal.DeletedAt = DateTime.UtcNow;
        journal.UpdatedAt = DateTime.UtcNow;
        journal.Version += 1;
        return journal;
    }
}