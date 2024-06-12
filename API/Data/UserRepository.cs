using API.Interfaces;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data 
{
    public class UserRepository : IUserRepository 
    {
        private readonly DataContext _context;
        public UserRepository(DataContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync() 
        {
            return await _context.Users.ToListAsync();
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser> GetUserByUsernameAsync(string username) 
        { 
            return await _context.Users.SingleOrDefaultAsync(x => x.UserName == username); // or FirstOrDefault
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user) 
        {
            _context.Entry(user).State = EntityState.Modified;
            // this tells our EF tracker that sth has changed with the entity (user) that we passed in 
            // we're not saving anything at this point 
            // we're just informing the EF tracker that an entity has been updated 

            // it's debatable whether we need this
            // bc when we make changes to an entity inside any of our methods, then EF is automatically going to track any changes
        }
    }
}