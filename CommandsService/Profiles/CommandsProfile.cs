﻿using AutoMapper;
using CommandsService.Dtos;
using CommandsService.Models;
using PlatformService;

namespace CommandsService.Profiles;

public class CommandsProfile : Profile
{
    public CommandsProfile()
    {
        // Source -> Target
        CreateMap<Platform, PlatformReadDto>();
        CreateMap<CommandCreateDto, Command>();
        CreateMap<Command, CommandReadDto>();
        CreateMap<PlatformPublishedDto, Platform>()
            .ForMember(dest => dest.ExternalID, opt => opt.MapFrom(src => src.Id));
        CreateMap<GrpcPlatformModel, Platform>()
            .ForMember(dest => dest.ExternalID, opts => opts.MapFrom(src => src.PlatformID))
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => src.Name))
            .ForMember(dest => dest.Commands, opts => opts.Ignore());
    }
}