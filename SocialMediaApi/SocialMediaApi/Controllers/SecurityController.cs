using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMediaApi.Data;
using SocialMediaApi.ModelClass.DTO;
using SocialMediaApi.ModelClass.Entities;
using SocialMediaApi.Token;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaApi.Controllers
{
    [Route("api/Security")]
    [ApiController]
    public class SecurityController : ControllerBase
    {
        private readonly DatabaseContext _context;
        private readonly ITokenService _token;

        public SecurityController(DatabaseContext context , ITokenService token)
        {
            _context = context;
            _token = token;
        }

        [HttpPost("Registration")]

        public async Task<ActionResult<TokenDTO>>Registration(RegisterDTO registerDTO)
        {
            try
            {
                if (await IsExist(registerDTO.UserName)) return BadRequest("UserName Already Taken!!!");

                using var hmac = new HMACSHA512();

                var user = new AppUser
                {
                    UserName = registerDTO.UserName,
                    PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                    PasswordSalt = hmac.Key,
                    KnownAs = registerDTO.KnownAs,
                    Created = registerDTO.Created,
                    LastActive = registerDTO.LastActive,
                    City = registerDTO.City,
                    Country = registerDTO.Country,
                    DateOfBirth = registerDTO.DateOfBirth,
                    Gender = registerDTO.Gender
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();

                return new TokenDTO
                {
                    UserName = user.UserName,
                    Token = _token.CreateToken(user)
                };
            }
            catch (Exception)
            {

                return BadRequest("Something went Wrong Please Try Again");
            }
           
        }

        [HttpPost ("Login")]
        public async Task<ActionResult<TokenDTO>>Login(LoginDTO loginDTO)
        {
          
                var user = await _context.Users
                    //.Include(p => p.Photos)
                    .SingleOrDefaultAsync(p => p.UserName == loginDTO.UserName);

                if (user == null) return BadRequest("Invalid UserName");

                using var hmac = new HMACSHA512(user.PasswordSalt);
                var computerhas = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                for (int i = 0; i < computerhas.Length; i++)
                {
                    if (computerhas[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
                }
                return new TokenDTO
                {
                    UserName = user.UserName,
                    Token = _token.CreateToken(user),
                    //PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
          
          
        }






        private async Task<bool> IsExist(string UserName)
        {
            return await _context.Users.AnyAsync(x => x.UserName == UserName);
        }
    }
}
