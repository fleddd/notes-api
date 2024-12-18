using api.Models;

namespace api.Dtos;

public class UserDto
{
    public string? Id { get; set; } 
    public string? UserName { get; set; }
    public string? Email { get; set; }
    public List<Note> Notes { get; set; } = [];
    
    public int AmountOfNotes { get; set; }
}