using Microsoft.AspNetCore.Mvc;

namespace DTO_Pagination_Filtering_Mapping;

[ApiController]
[Route("/api/journals")]
public sealed class JournalController : ControllerBase
{
    private readonly IJournalRepository _journalRepository;

    public JournalController(IJournalRepository journalRepository)
    {
        _journalRepository = journalRepository;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetJournals([FromQuery] JournalFilter filter)
    {
        var result = _journalRepository.GetAllJournals(filter);
        return Ok(ApiResponse<PaginationResponse<IEnumerable<JournalReadDto>>>.Success(null!, result));
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetJournalById(int id)
    {
        var journal = _journalRepository.GetJournalById(id);
        return journal != null
            ? Ok(ApiResponse<JournalReadDto>.Success(null!, journal))
            : NotFound(ApiResponse<JournalReadDto>.Fail(null!, null));
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult CreateJournal([FromBody] JournalCreateDto journalCreateDto)
    {
        var result = _journalRepository.CreateJournal(journalCreateDto);
        return result
            ? Ok(ApiResponse<bool>.Success(null!, result))
            : BadRequest(ApiResponse<bool>.Fail(null!, result));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult UpdateJournal([FromBody] JournalUpdateDto journalUpdateDto)
    {
        var result = _journalRepository.UpdateJournal(journalUpdateDto);
        return result
            ? Ok(ApiResponse<bool>.Success(null!, result))
            : NotFound(ApiResponse<bool>.Fail(null!, result));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteJournal(int id)
    {
        var result = _journalRepository.DeleteJournal(id);
        return result
            ? Ok(ApiResponse<bool>.Success(null!, result))
            : NotFound(ApiResponse<bool>.Fail(null!, result));
    }
}
