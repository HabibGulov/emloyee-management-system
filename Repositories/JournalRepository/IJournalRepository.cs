public interface IJournalRepository
{
    PaginationResponse<IEnumerable<JournalReadDto>> GetAllJournals(JournalFilter journalFilter);
    JournalReadDto? GetJournalById(int id);
    bool CreateJournal(JournalCreateDto journalCreateDTO);
    bool DeleteJournal(int id);
    bool UpdateJournal(JournalUpdateDto journalUpdateDTO);
}

