using api.Dtos;
using api.Models;

namespace api.Mappers;

public static class NoteMapper
{
    public static NoteDto ToNoteDto(this Note note)
    {
        return new NoteDto
        {
            Id = note.Id,
            Title = note.Title,
            Description = note.Description,
            Tag = note.Tag,
            CreatedAt = note.CreatedAt,
            IsDone = note.IsDone,
            UserId = note.UserId
        };
    }
    public static Note ToNoteFromCreateNoteDto(this CreateNoteRequestDto requestDto)
    {
        return new Note
        {
            Title = requestDto.Title,
            Description = requestDto.Description,
            Tag = requestDto.Tag,
        };
    }
    public static Note ToNoteFromUpdateNoteDto(this UpdateNoteRequestDto requestDto)
    {
        return new Note
        {
            Title = requestDto.Title,
            Description = requestDto.Description,
            Tag = requestDto.Tag,
            IsDone = requestDto.IsDone
        };
    }
}