using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize]
public class UsersController : BaseApiController

{
    private readonly DataContext _context; // this is scoped to the http request 

    public UsersController(DataContext context) {
        _context = context;
    }

    [AllowAnonymous] // a directive to specify that a particular action/controller does not require authentication.
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    // ActionResult is a base class that can represent various HTTP responses, 
    // such as 200 OK, 400 Bad Request, etc.
    {
        var users = await _context.Users.ToListAsync();
        // ToList() is a LINQ method that executes the query and retrieves all AppUser records 
        // from the database, returning them as a list.
        return users;
        // Because the return type is ActionResult<IEnumerable<AppUser>>, 
        // the framework automatically wraps the returned list in an Ok result, corresponding to HTTP status code 200 (OK).
    }

    [HttpGet("{id}")]// /api/users/2
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null)
        {
            return NotFound();
        }
        return user;
    }


}