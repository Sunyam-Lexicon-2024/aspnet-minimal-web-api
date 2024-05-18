using AutoMapper;
using MinimalWebAPI.Models;
using MinimalWebAPI.Models.APIModels;

namespace MinimalWebAPI.MappingProfiles;

public class ItemMappingProfile : Profile
{
    public ItemMappingProfile()
    {
        CreateMap<Item, ItemAPIModel>().ReverseMap();
    }
}