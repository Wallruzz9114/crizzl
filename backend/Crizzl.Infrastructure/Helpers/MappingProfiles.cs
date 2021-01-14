using System.Linq;
using AutoMapper;
using Crizzl.Application.Settings;
using Crizzl.Domain.DTOs.User;
using Crizzl.Domain.Entities;
using Crizzl.Domain.ViewModels;

namespace Crizzl.Infrastructure.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<User, UserDetailsForListDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetAge()))
                .ForMember(dest => dest.MainPhotoURL, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).URL));

            CreateMap<User, UserDetailsDTO>()
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateOfBirth.GetAge()))
                .ForMember(dest => dest.MainPhotoURL, opt => opt.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).URL));

            CreateMap<Photo, PhotoDetails>();
            CreateMap<Photo, PhotoDetails>();
            CreateMap<FileParameters, Photo>();
        }
    }
}