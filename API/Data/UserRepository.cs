using API.Interfaces;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using API.DTOs;

namespace API.Data 
{
    public class UserRepository : IUserRepository 
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
                // when we use Projection (ProjectTo)
                // we dont need to eagerly load the related entities bc projection is taking care of that
                // 
        }
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync() 
        {
            return await _context.Users
                .Include(p => p.Photos)
                .ToListAsync();
            // ToList() is a LINQ method that executes the query and retrieves all AppUser records 
            // from the database, returning them as a list.
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<AppUser> GetUserByUsernameAsync(string username) 
        { 
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username); // or FirstOrDefault
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