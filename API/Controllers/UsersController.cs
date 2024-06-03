using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

public class UsersController : BaseApiController
// ControllerBase: A base class for an MVC controller without view support.
// 1. ControllerBase is designed for building APIs, providing a variety of methods and properties 
// to simplify the development of RESTful services.
// 2. It does not include view support, which is provided by the Controller class that inherits from ControllerBase.

{
    private readonly DataContext _context; // this is scoped to the http request 

    public UsersController(DataContext context) {
        _context = context;
    }

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