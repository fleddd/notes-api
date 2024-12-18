using System.Security.Claims;
using api.Data;
using api.Dtos;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace api.Repositories;

public class NotesRepository : INoteRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public NotesRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }

    // Example: Method to access claims values
    private string? GetUserIdFromClaims()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        if ((user == null) || (!user.Identity?.IsAuthenticated ?? true))
        {
            return null;
        }

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier); // Or use your specific claim type
        return userIdClaim?.Value;
    }


    public async Task<List<Note>> GetAllAsync(QueryObject queryObject)
    {
        var notes = _context.Notes.AsNoTracking().AsQueryable();
        var userId = GetUserIdFromClaims();
        if (!string.IsNullOrEmpty(userId))
        {
            notes = notes.Where(x => x.UserId == userId);
        }
        
        if (!string.IsNullOrEmpty(queryObject.Title))
        {
            notes = notes.Where(x => x.Title.ToLower().Contains(queryObject.Title.ToLower()));
        }

        return await notes
            .Skip((queryObject.Page - 1) * queryObject.PageSize)
            .Take(queryObject.PageSize)
            .OrderBy(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<Note?> GetByIdAsync(int id)
    {
        var note = await _context.Notes.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        var userId = GetUserIdFromClaims();
        return note?.UserId != userId ? null : note;
    }

    public async Task<Note?> CreateAsync(Note newNote)
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return null;
        
        // Create the new note
        newNote.UserId = userId;
        await _context.AddAsync(newNote);
        await _context.SaveChangesAsync();
        return newNote;
    }

    public async Task<Note?> UpdateAsync(Note updatedNote, int noteId)
    {
        var existingNote = await _context.Notes.FindAsync(noteId);
        if (existingNote == null) return null;
    
        
        existingNote.Title = updatedNote.Title;
        existingNote.Description = updatedNote.Description;
        existingNote.Tag = updatedNote.Tag;
        existingNote.IsDone = updatedNote.IsDone;

        await _context.SaveChangesAsync();
        return existingNote;
    }

    public async Task<Note?> DeleteAsync(int id)
    {
        var note = await _context.Notes.FindAsync(id);
        var userId = GetUserIdFromClaims();
        if (note?.UserId != userId || note is null)
        {
            return null;
        }
        
        // Delete the existing note
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
        return note;
    }

    public async Task<int?> GetAllNotesCountAsync()
    {
        var userId = GetUserIdFromClaims();
        if (userId == null) return null;
        
       return await _context.Notes.AsNoTracking().Where(x => x.UserId == userId).CountAsync();
    }
}