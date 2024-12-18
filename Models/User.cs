using Microsoft.AspNetCore.Identity;

namespace api.Models;

public class User : IdentityUser
{
    public List<Note> Notes { get; set; }
}