using SocialMediaApi.Helper;
using SocialMediaApi.ModelClass.DTO;
using SocialMediaApi.ModelClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Interface
{
    public interface IUserRepository
    {
        Task<AppUser> AddUserAsync(AppUser user);
        Task<AppUser> GetUserByNameAsync(string userName);
        Task<AppUser> GetUserById(int id);
        Task<PagedList<MemberDTO>> GetMembersAsync(UserParams userParams);
        Task<MemberDTO> GetMemberByNameAsync(string memberName);
        void Update(AppUser user);
        Task<bool> SaveAllAsync();
        Task<bool> IsExists(string userName);
    }
}
