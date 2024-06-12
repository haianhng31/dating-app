using API.Data;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository) 
    {
       _userRepository = userRepository;
    }

    // [AllowAnonymous] // a directive to specify that a particular action/controller does not require authentication.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    // ActionResult is a base class that can represent various HTTP responses, 
    // such as 200 OK, 400 Bad Request, etc.
    {
        return Ok(await _userRepository.GetUsersAsync());
    }

    [HttpGet("{username}")]// /api/users/lisa
    public async Task<ActionResult<AppUser>> GetUser(string username)
    {
        return await _userRepository.GetUserByUsernameAsync(username);
        // if (user == null)
        // {
        //     return NotFound();
        // }
        // return user;
    }

}