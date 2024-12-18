using api.Dtos;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly SignInManager<User> _signInManager;

    public AccountController(UserManager<User> userManager, ITokenService tokenService, SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
    }


    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest();

            var user = new User
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName
            };

            var createdUser = await _userManager.CreateAsync(user, registerDto.Password);

            if (createdUser.Succeeded)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, "User");
                if (!roleResult.Succeeded) return BadRequest(roleResult.Errors);
                return Ok(
                        new NewUserDto
                        {
                            Email = user.Email,
                            UserName = user.UserName,
                            Token = _tokenService.CreateToken(user)
                        }
                    );
            }
            else
            {
                return BadRequest(createdUser.Errors);
            }
        }
        catch (Exception e)
        {
            return BadRequest("Something bad has happened.");
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);
        
        if(user is null) return StatusCode(400, "Invalid password or username provided.");
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password, false);
        if(!result.Succeeded) return StatusCode(400, "Invalid password or username provided.");
        
        return Ok(new
        {   
            user.Email,
            user.UserName,
            Token = _tokenService.CreateToken(user)
        });
    }
    
}