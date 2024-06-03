using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers 
{
    public class AccountController : BaseApiController 
    {
        private readonly DataContext _context;

        public AccountController(DataContext context) 
        {
            _context = context;
        }

        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<AppUser>> Register(string username, string password) 
        {
            using var hmac = new HMACSHA512();
            // when we create a new instance of a class like we're doing here
            // that's going to consume some space in memory 
            // if use "Using" keyword -> gonna dispose of it automatically 

            var user = new AppUser
            {
                UserName = username, 
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
                PasswordSalt = hmac.Key
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }


    }
}