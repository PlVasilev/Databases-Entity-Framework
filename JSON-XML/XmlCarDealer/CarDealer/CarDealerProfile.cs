using AutoMapper;
using CarDealer.Dtos.Import;
using CarDealer.Models;

namespace CarDealer
{
    public class CarDealerProfile : Profile
    {
        public CarDealerProfile()
        {
            this.CreateMap<SuplierDto, Supplier>();

            this.CreateMap<PartDto, Part>();

            this.CreateMap<CustomerDto, Customer>();

            this.CreateMap<SaleDto, Sale>();
        }
    }
}
