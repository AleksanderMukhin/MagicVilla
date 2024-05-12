using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;

namespace MagicVilla_VillaAPI;

public class MappingConfig:Profile
{
    public MappingConfig()
    {
        CreateMap<Villa, VillaDto>();
        CreateMap<VillaDto, Villa>();
        
        CreateMap<Villa, VillaCreateDto>().ReverseMap();
        CreateMap<Villa, VillaUpdateDto>().ReverseMap();
        
        CreateMap<VillaNomber, VillaNomberDTO>();
        CreateMap<VillaNomberDTO, VillaNomber>();
        
        CreateMap<VillaNomberDTO, VillaNomberUpdateDTO>().ReverseMap();
        CreateMap<VillaNomberDTO, VillaNomberCreateDTO>().ReverseMap();
        
        CreateMap<VillaNomber, VillaNomberUpdateDTO>().ReverseMap();
        CreateMap<VillaNomber, VillaNomberCreateDTO>().ReverseMap();
    }
}