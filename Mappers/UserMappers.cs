using api.Dtos;
using api.Models;

namespace api.Mappers;

public static class UserMappers
{
    public static UserDto ToUserDto(this User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            UserName = user.UserName,
            Notes = user.Notes,
            AmountOfNotes = user.Notes.Count
        };
    }
}