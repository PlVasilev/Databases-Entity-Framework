using System;
using AutoMapper;

namespace AutoMappingObjectsLAB
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            //codebeoutify.org
            //Install-Package AutoMapper

            // AutoMapper offers a static service for use and configuration

            // Add mappings between objects and DTOs
            Mapper.Initialize(cfg => cfg.CreateMap<Product, ProductDTO>());

            //Properties will be mapped by name
            var product = context.Products.FirstOrDefault();
            ProductDTO dto = Mapper.Map<ProductDTO>(product);

            //Multiple Mappings - You can configure all mapping configurations at once
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Product, ProductDTO>();
                cfg.CreateMap<Order, OrderDTO>();
                cfg.CreateMap<Client, ClientDTO>();
                cfg.CreateMap<SupportTicket, TicketDTO>();
            });

            //Map properties that don't match naming convention
            Mapper.Initialize(cfg =>
                cfg.CreateMap<Product, ProductDTO>()
                    .ForMember(dto => dto.StockQty,
                        opt => opt.MapFrom(src =>
                            src.ProductStocks.Sum(p => p.Quantity))));

            //Flattening Complex Properties -- AutoMapper can also be used to flatten complex properties
            Mapper.Initialize(cfg =>
                cfg.CreateMap<Event, CalendarEventViewModel>()
                    .ForMember(dest => dest.Date,
                        opt => opt.MapFrom(src => src.Date.Date))
                    .ForMember(dest => dest.Hour,
                        opt => opt.MapFrom(src => src.Date.Hour))
                    .ForMember(dest => dest.Minute,
                        opt => opt.MapFrom(src => src.Date.Minute)));

            //Flattening Complex Objects Flattening of related objects is automatically supported
            //public class OrderDTO
            //{
            //    public string ClientName { get; set; }
            //    public decimal Total { get; set; }
            //}

            //AutoMapper understands ClientName is the Name of a Client
            Mapper.Initialize(cfg => cfg.CreateMap<Order, OrderDTO>());
            OrderDTO dto = Mapper.Map<Order, OrderDTO>(order);

            //AutoMapper understands ClientName is the Name of a Client, but to unflatten it, it needs ReverseMap
            Mapper
                .Initialize(cfg => cfg.CreateMap<Order, OrderDTO>()
                    .ReverseMap());

            //Mapping Collections
            // EF Core uses IQueryable<T> for all DB operations
            //     AutoMapper can work with IQueryable < T > to map classes
            //     Using AutoMapper to map an entire DB collection:
            var posts = context.Posts
                .Where(p => p.Author.Username == "gosho") // IQueryable<Post>
                .ProjectTo<PostDto>() //IQueryable<PostDto>
                .ToArray();
            //Works like an automatic .Select()
            //EF Core generates optimized SQL(like with an anonymous object)

            //AutoMapper.Collection
            //Adds ability to map collections to existing collections without re-creating the collection object
            //Will Add/ Update / Delete items from a preexisting collection object based on user
            //defined equivalency between the collection's generic item type from the source collection and the destination collection
            Mapper.Initialize(cfg => cfg.AddCollectionMappers());

            //AutoMapper.Collection
            //Adds ability to map collections to existing collections without re-creating the collection object
            //    Adding equivalence to objects is done with EqualityComparison extended from the IMappingExpression class
            cfg.CreateMap<OrderItemDTO, OrderItem>()
                .EqualityComparison((odto, o) => odto.ID == o.ID);
            //If ID's match will map OrderDTO to Order
            //If OrderDTO exists and Order doesn't add to collection
            //If Order exists and OrderDTO doesn't remove from collection

            //AutoMapper.Collection.EntityFrameworkCore
            //Automapper.Collection.EntityFrameworkCore will help you mapping of EntityFrameowrk Core DbContext-object
            Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.SetGeneratePropertyMaps
                    <GenerateEntityFrameworkCorePrimaryKeyPropertyMaps<Context>>();
                // Configuration code
            });
            //Comparing to a single existing Entity for updating
            dbContext.Orders.Persist().InsertOrUpdate<OrderDTO>(newOrderDto);
            dbContext.Orders.Persist().InsertOrUpdate<OrderDTO>(existingOrderDto);
            dbContext.Orders.Persist().Remove<OrderDTO>(deletedOrderDto);
            dbContext.SubmitChanges();

            //Inheritance Mapping
            //Inheritance chains are defined via Include()
            //AutoMapper chooses the most appropriate child class
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Order, OrderDto>()
                    .Include<OnlineOrder, OnlineOrderDto>()
                    .Include<MailOrder, MailOrderDto>();
                cfg.CreateMap<OnlineOrder, OnlineOrderDto>();
                cfg.CreateMap<MailOrder, MailOrderDto>();
            });

            // Mapping Profiles
            // We can extract our configuration to a class (called a profile)
            //public class ForumProfile : Profile
            //{
            //    public ForumProfile()
            //    {
            //        CreateMap<Post, PostDto>();
            //        CreateMap<Category, CategoryDto>();
            //    }
            //}
            //Using our configuration class:
            Mapper.Initialize(cfg => cfg.AddProfile<ForumProfile>());

            //Nested Mapping
            //AutoMapper uses another type map, where the source member type and destination member type are also configured in the mapping configuration
            //    In that case, we would need to configure the additional source/ destination type mappings
            //Order of configuring types does not matter
            //    Call to Map does not need to specify any inner type mappings, only the type map to use for the source value passed in
            //With both flattening and nested mappings, we can create a variety of destination shapes to suit whatever our needs may be

            //Mapping Collections
            //AutoMapper only requires configuration of element types, not of any array or list type that might be used
            //All the basic generic collection types are supported
            //When mapping to an existing collection, the destination collection is cleared first
            //When mapping a collection property, if the source value is null AutoMapper will map the destination 
            //    field to an empty collection rather than setting the destination value to null
            //This behavior can be changed by setting the AllowNullCollections

        }
    }
}
