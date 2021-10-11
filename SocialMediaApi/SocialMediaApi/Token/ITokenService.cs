using SocialMediaApi.ModelClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Token
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
