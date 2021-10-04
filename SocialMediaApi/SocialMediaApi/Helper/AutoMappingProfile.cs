using AutoMapper;
using SocialMediaApi.Extentions;
using SocialMediaApi.ModelClass.DTO;
using SocialMediaApi.ModelClass.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialMediaApi.Helper
{
    public class AutoMappingProfile : Profile
    {
        public AutoMappingProfile()
        {
            CreateMap<AppUser,MemberDTO>()
                .ForMember(des => des.PhotoUrl, opt => opt.MapFrom(x => x.Photos.FirstOrDefault(p => p.IsMain).Url))
                .ForMember(des => des.Age, opt => opt.MapFrom(x => x.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDTO>();
            CreateMap<MemberUpdateDTO, AppUser>();

        }
    }
}
