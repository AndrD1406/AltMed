﻿using AltMed.BusinessLogic.Dtos;
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

        CreateMap<Publication, PublicationDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author!.Name))
            .ForMember(dest => dest.AuthorLogo, opt => opt.MapFrom(src => src.Author!.Logo));

        CreateMap<Publication, CommentDto>()
            .ForMember(d => d.ParentId, opt => opt.MapFrom(p => p.ParentId!.Value))
            .ForMember(d => d.CreatedAt, opt => opt.MapFrom(p => p.PostedAt))
            .ForMember(d => d.AuthorName, opt => opt.MapFrom(p => p.Author!.Name))
            .ForMember(d => d.AuthorLogo, opt => opt.MapFrom(p => p.Author!.Logo))
            .ForMember(d => d.Image64, opt => opt.MapFrom(p => p.Image64));

        CreateMap<Like, LikeDto>()
            .ForMember(dest => dest.PublicationId,
                       opt => opt.MapFrom(src => src.PublicationId))
            .ForMember(dest => dest.AuthorName,
                       opt => opt.MapFrom(src => src.Author!.Name));

        CreateMap<ApplicationUser, ProfileDto>()
            .ForMember(d => d.UserId, o => o.MapFrom(u => u.Id))
            .ForMember(d => d.Name, o => o.MapFrom(u => u.Name))
            .ForMember(d => d.Logo, o => o.MapFrom(u => u.Logo))
            .ForMember(d => d.PublicationIds,
                       o => o.MapFrom(u => u.Publications
                                           .Where(p => p.AuthorId == u.Id)
                                           .Select(p => p.Id)));

        CreateMap<CommentCreateDto, Publication>()
            .ForMember(dest => dest.AuthorId, opt => opt.MapFrom(src => src.AuthorId))
            .ForMember(dest => dest.ParentId, opt => opt.MapFrom(src => src.PublicationId))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
            .ForMember(dest => dest.Image64, opt => opt.MapFrom(src => src.Image64))
            .ForMember(dest => dest.PostedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
