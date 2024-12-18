using api.Dtos;
using api.Helpers;

namespace api.Interfaces;

public interface INotesService
{
    Task<NoteDto?> CreateNote(CreateNoteRequestDto createNoteRequestDto);
    Task<NoteDto?> DeleteNote(int id);
    Task<NoteDto?> UpdateNote(int id, UpdateNoteRequestDto updateNoteRequestDto);
    Task<NoteDto?> GetNoteById(int id);
    Task<List<NoteDto>> GetAllNotes(QueryObject queryObject);
    Task<int?> GetAllNotesCount();
}