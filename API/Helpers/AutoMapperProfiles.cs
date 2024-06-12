using AutoMapper;
using API.Entities; 
using API.DTOs;
using API.Extensions;

namespace API.Helpers 
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<AppUser, MemberDto>()
                .ForMember(
                    destination => destination.PhotoUrl, 
                    options => options.MapFrom(src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(
                    des => des.Age, 
                    opt => opt.MapFrom(src => src.DateOfBirth.CalculateAge()));
            CreateMap<Photo, PhotoDto>();
        }
    }
}