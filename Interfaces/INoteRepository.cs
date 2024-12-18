using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Interfaces;

public interface INoteRepository
{
    Task<List<Note>> GetAllAsync(QueryObject queryObject);
    Task<Note?> GetByIdAsync(int id);
    Task<Note?> CreateAsync(Note note);
    Task<Note?> UpdateAsync(Note note, int id);
    Task<Note?> DeleteAsync(int id); 
    Task<int?> GetAllNotesCountAsync();
}