using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.Data;
using SocialMediaApi.Helper;
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

        public async Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams)
        {
            var query = _context.Users
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .AsNoTracking();

            return await PagedList<MemberDTO>.CreateAsync(query, userParams.PageNumber, userParams.PageSize);

        }

        public async Task<MemberDTO> GetMemberByNameAsync(string memberName)
        {
            return await _context.Users.Where(p => p.UserName.ToLower() == memberName.ToLower())
                .ProjectTo<MemberDTO>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

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
