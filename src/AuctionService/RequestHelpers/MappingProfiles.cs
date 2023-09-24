using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts;

namespace AuctionService.RequestHeplers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Auction, AuctionDto>().IncludeMembers(x => x.Item);
        CreateMap<Item, AuctionDto>();
        CreateMap<CreateAuctionDto, Auction>().ForMember(d => d.Item, o => o.MapFrom(s => s));
        CreateMap<CreateAuctionDto, Item>();
        CreateMap<AuctionDto, AuctionCreated>();
        CreateMap<Auction, AuctionUpdated>()
            .ForMember(x => x.Id, src => src.MapFrom(e => e.Id))
            .ForMember(x => x.Make, src => src.MapFrom(e => e.Item.Make))
            .ForMember(x => x.Model, src => src.MapFrom(e => e.Item.Model))
            .ForMember(x => x.Mileage, src => src.MapFrom(e => e.Item.Mileage))
            .ForMember(x => x.Year, src => src.MapFrom(e => e.Item.Year));

        CreateMap<Auction, AuctionDeleted>();
    }
}