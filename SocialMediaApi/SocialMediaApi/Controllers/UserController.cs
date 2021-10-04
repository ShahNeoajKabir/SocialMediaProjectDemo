using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SocialMediaApi.Extentions;
using SocialMediaApi.Interface;
using SocialMediaApi.ModelClass.DTO;
using SocialMediaApi.ModelClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Controllers
{
    [Route("api/User")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<AppUser>>AddUser(AppUser user)
        {
            if (user.UserName == null && user.KnownAs == null) return BadRequest("Null Not Acceptable");
            if (await _userRepository.IsExists(user.UserName)) return BadRequest("User Already Taken");
            var users = await _userRepository.AddUserAsync(user);

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Can't Added User. Please Try Again");
        }


        [HttpGet("GetAll")]

        public async Task<ActionResult<IEnumerable<MemberDTO>>> GetAll()
        {
            var user = await _userRepository.GetAllMemberAsync();
            return Ok(user);
        }

        [HttpGet ("{membername}")]
        public async Task<ActionResult<MemberDTO>>GetMemberByAsync(string memberName)
        {
            return Ok(await _userRepository.GetMemberByNameAsync(memberName));
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult> UpdateUser(MemberUpdateDTO memberUpdateDTO)
        {
            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());
            _mapper.Map(memberUpdateDTO, user);
            _userRepository.Update(user);
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed To Update");

        }
    }
}
