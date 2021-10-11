using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
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

        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ITokenService _token;
        private readonly IMapper _mapper;

        public SecurityController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager, ITokenService token
            ,IMapper mapper
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _token = token;
            _mapper = mapper;
        }

        [HttpPost("Registration")]

        public async Task<ActionResult<TokenDTO>>Registration(RegisterDTO registerDTO)
        {
            try
            {
                if (await IsExist(registerDTO.UserName)) return BadRequest("UserName Already Taken!!!");


                var user = _mapper.Map<AppUser>(registerDTO);

                user.UserName = registerDTO.UserName.ToLower();
                var result = await _userManager.CreateAsync(user, registerDTO.Password);
                if (!result.Succeeded) return BadRequest(result.Errors);

                var roleresult = await _userManager.AddToRoleAsync(user, "Member");
                if (!roleresult.Succeeded) return BadRequest(roleresult.Errors);

                return new TokenDTO
                {
                    UserName = user.UserName,
                    Token =await _token.CreateToken(user)

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
          
                var user = await _userManager.Users
                    .Include(p => p.Photos)
                    .SingleOrDefaultAsync(p => p.UserName == loginDTO.UserName.ToLower());

                if (user == null) return BadRequest("Invalid UserName");

            var result = await _signInManager.CheckPasswordSignInAsync(user, loginDTO.Password, false);

            if (!result.Succeeded) return Unauthorized();

                return new TokenDTO
                {
                    UserName = user.UserName,
                    Token =await _token.CreateToken(user),
                    PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
                };
          
          
        }






        private async Task<bool> IsExist(string UserName)
        {
            return await _userManager.Users.AnyAsync(x => x.UserName == UserName);
        }
    }
}
