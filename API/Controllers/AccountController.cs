using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class AccountController : BaseApiController 
{
    private readonly DataContext _context;
    private readonly ITokenService _tokenService;

    public AccountController(DataContext context, ITokenService tokenService) 
    {
        _context = context;
        _tokenService = tokenService;
    }

    [HttpPost("register")] // api/account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto) 
    // 1 of the power of the [ApiController] is it binds automatically to the object that we have inside our method
    // in order for it to bind to the registerDto, then the Property name needs to match exactly 
    // with what we have in our registered DTO 
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");

        using var hmac = new HMACSHA512();
        // when we create a new instance of a class like we're doing here
        // that's going to consume some space in memory 
        // if use "Using" keyword -> gonna dispose of it automatically 

        var user = new AppUser
        {
            UserName = registerDto.Username.ToLower(), 
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
            PasswordSalt = hmac.Key
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return new UserDto 
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) 
    {
        var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.Username);
        if (user == null) return Unauthorized();

        using var hmac = new HMACSHA512(user.PasswordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i]!= user.PasswordHash[i]) return Unauthorized("Invalid password");
        }
        // In C#, you can't directly compare two arrays using the == operator. 
        // When you use "computedHash != user.PasswordHash", you're comparing references to the arrays, not their contents. 
        // Since computedHash and user.PasswordHash are both byte arrays, 
        // the != operator checks if they refer to the same memory location, not if their contents are equal.

        return new UserDto 
        {
            Username = user.UserName,
            Token = _tokenService.CreateToken(user)
        };
    }

    private async Task<bool> UserExists(string username) 
    {
        return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
    }
}
