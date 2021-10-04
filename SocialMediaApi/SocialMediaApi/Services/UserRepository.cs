using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.Data;
using SocialMediaApi.Interface;
using SocialMediaApi.ModelClass.DTO;
using SocialMediaApi.ModelClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly DatabaseContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DatabaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AppUser> AddUserAsync(AppUser user)
        {
            await _context.Users.AddAsync(user);
            return user;
        }

        public async Task<IEnumerable<MemberDTO>> GetAllMemberAsync()
        {
            return await _context.Users
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();
        }

        public async Task<MemberDTO> GetMemberByNameAsync(string memberName)
        {
            return await _context.Users.Where(p => p.UserName == memberName)
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync();

        }

        public async Task<AppUser> GetUserById(int id)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(p => p.Id == id);
                
        }

        public async Task<AppUser> GetUserByNameAsync(string userName)
        {
            return await _context.Users
                .Include(p => p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == userName);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<bool> IsExists(string userName)
        {
            return await _context.Users.AnyAsync(p => p.UserName == userName);
        }
    }
}
