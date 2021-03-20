using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ProductShop.Models;
using ProductShop.Views.Dtos;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<User, UserSalesDto>().ForMember(x => x.SoldProduct, y => y.Ignore());
            CreateMap<Product, SoldProductDto>()
                .ForMember(x => x.BuyerFirstName, y => y.MapFrom(s => s.Buyer.FirstName))
                .ForMember(x => x.BuyerLastName, y => y.MapFrom(s => s.Buyer.LastName));
            
            //8
            CreateMap<User, UserDto>()
                .ForMember(x => x.SoldProducts, y => y.MapFrom(obj => obj));

            CreateMap<User, SoldProducts>()
                .ForMember(x => x.Products, y => y.MapFrom(obj => obj.ProductsSold.Where(x => x.Buyer != null)));

            CreateMap<Product, ProductsDto>();

            CreateMap<List<UserDto>, UsersAndProductsDto>()
                .ForMember(x => x.Users, y => y.MapFrom(obj => obj));
        }
    }
}
