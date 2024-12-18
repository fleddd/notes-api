using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Services;

public class NotesService : INotesService
{
    private readonly INoteRepository _noteRepository;
    private readonly UserManager<User> _userManager;

    public NotesService(INoteRepository noteRepository, UserManager<User> userManager)
    {
        _noteRepository = noteRepository;
        _userManager = userManager;
    }
    
    public async Task<NoteDto?> CreateNote(CreateNoteRequestDto createNoteDto)
    {
        var newNote = createNoteDto.ToNoteFromCreateNoteDto();
        var note = await _noteRepository.CreateAsync(newNote);
        return note.ToNoteDto();
    }

    public async Task<NoteDto?> DeleteNote(int id)
    {
        var note =  await _noteRepository.DeleteAsync(id);
        return note?.ToNoteDto();
    }

    public async Task<NoteDto?> UpdateNote(int id, UpdateNoteRequestDto updateNoteRequestDto)
    {
        var updateNote = updateNoteRequestDto.ToNoteFromUpdateNoteDto();
        var newNote = await _noteRepository.UpdateAsync(updateNote, id);
        return newNote?.ToNoteDto();
    }

    public async Task<NoteDto?> GetNoteById(int id)
    {
        var note = await _noteRepository.GetByIdAsync(id);
        return note?.ToNoteDto();
    }

    public async Task<List<NoteDto>> GetAllNotes(QueryObject queryObject)
    {
        var notes = await _noteRepository.GetAllAsync(queryObject);
        return notes.Select(x => x.ToNoteDto()).ToList();
    }

    public async Task<int?> GetAllNotesCount()
    {
        return await _noteRepository.GetAllNotesCountAsync();
    }

    
}