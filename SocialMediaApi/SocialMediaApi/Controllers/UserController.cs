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
        private readonly IPhotoService _photoService;

        public UserController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _photoService = photoService;
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

        [HttpGet ("{membername}", Name = "GetUserName")]
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

        [HttpPost("AddPhoto")]

        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {

            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());
            var result = await _photoService.AddPhotoAsync(file);
            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId

            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);
            if (await _userRepository.SaveAllAsync())
            {
                return CreatedAtRoute("GetUserName", new { username = user.UserName }, _mapper.Map<PhotoDTO>(photo));
            }
            return BadRequest("Problem Adding Photo");
        }

        [HttpPut("set-main-photo/{photoId}")]
        public async Task<ActionResult> SetMainPhoto(int photoId)
        {
            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo.IsMain) return BadRequest("This is already your main photo");

            var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
            if (currentMain != null) currentMain.IsMain = false;
            photo.IsMain = true;

            if (await _userRepository.SaveAllAsync()) return NoContent();

            return BadRequest("Failed to set main photo");
        }

        [HttpDelete("delete-photo/{photoId}")]
        public async Task<ActionResult> DeletePhoto(int photoId)
        {
            var user = await _userRepository.GetUserByNameAsync(User.GetUserName());

            var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            user.Photos.Remove(photo);

            if (await _userRepository.SaveAllAsync()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}
