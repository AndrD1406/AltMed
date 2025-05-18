using AltMed.BusinessLogic.Dtos;
using AltMed.BusinessLogic.Dtos.Identity;
using AltMed.DataAccess.Identity;
using AltMed.DataAccess.Migrations;
using AltMed.DataAccess.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AltMed.BusinessLogic.Util;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<RegisterDto, ApplicationUser>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

        CreateMap<PublicationCreateDto, Publication>()
            .ForMember(dest => dest.ParentId, opt => opt.Ignore());
    }
}
