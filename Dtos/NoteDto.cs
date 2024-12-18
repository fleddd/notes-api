using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class NoteDto
{
    public int Id { get; set; }
    [Required]
    [MinLength(3, ErrorMessage = "Title must be at least 3 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be less than 20 characters long.")]
    public string Title { get; set; } = string.Empty;
    
    [MaxLength(1000, ErrorMessage = "Description must be less than 1000 characters long.")]
    [Required]
    public string Description { get; set; } = string.Empty;
    [Required]
    [TagValidation]
    public string Tag { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsDone { get; set; } = false;
    public string? UserId { get; set; }
}