using AutoMapper;
using Talabat.API.Dtos;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Calender;
using UserAddress = Talabat.Core.Entities.Identity.Address;

namespace Talabat.API.Helpers
{
    public class MappingProfiles: Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductToReturnDto>()
                .ForMember(d => d.Brand, O => O.MapFrom(s => s.Brand.Name))
                .ForMember(d => d.Category, O => O.MapFrom(s => s.Category.Name))
                //.ForMember(d => d.PictureUrl, O => O.MapFrom(s => $"{""}/{s.PictureUrl}"));
                .ForMember(d => d.PictureUrl, O => O.MapFrom<ProductPictureUrlResolver>());
            //first parameter is the source and the second is the destination
            //here the method need to know that ProductToReturnDto take its value from Product

            CreateMap<UserAddress, AddressDto>().ReverseMap();
            //ReverseMap() is used to map the other way around
            CreateMap<CustomerBasketDto, CustomerBasket>();
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<WorkScheduleFromUserDto, WorkSchedule>();
            CreateMap<WorkSchedule, WorkScheduleFromDatabaseDto>();
        }
    }
}
