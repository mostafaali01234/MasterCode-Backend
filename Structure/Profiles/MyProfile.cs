using AutoMapper;
using Entities.DTO;
using Entities.Models;

namespace Structure.Profiles
{
    public class MyProfile : Profile
    {
        public MyProfile()
        {
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartCreateDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderCreateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailCreateDto>().ReverseMap();

            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartUpdateDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderUpdateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailUpdateDto>().ReverseMap();

            CreateMap<Product, ProductDisplayDto>().ForMember(pts => pts.CategoryName, opt => opt.MapFrom(ps => ps.Category.Name)).ReverseMap();
            CreateMap<Category, CategoryDisplayDto>().ReverseMap();
            CreateMap<Customer, CustomerDisplayDto>().ForMember(pts => pts.AddedUserName, opt => opt.MapFrom(ps => ps.ApplicationUser.UserName)).ReverseMap();
            CreateMap<ApplicationUser, DisplayUserDto>()
                .ForMember(pts => pts.JobName, opt => opt.MapFrom(ps => ps.Job.Name))
                .ForMember(pts => pts.Phone, opt => opt.MapFrom(ps => ps.PhoneNumber))
                .ReverseMap();
            CreateMap<OrderHeader, OrderHeaderDisplayDto>()
                .ForMember(pts => pts.CustomerName, opt => opt.MapFrom(ps => ps.Customer.CustomerName))
                .ForMember(pts => pts.ApplicationUserName, opt => opt.MapFrom(ps => ps.ApplicationUser.Name))
                .ForMember(pts => pts.TechName, opt => opt.MapFrom(ps => ps.Tech.Name))
                .ReverseMap();
            CreateMap<OrderDetail, OrderDetailDisplayDto>().ForMember(pts => pts.ProductName, opt => opt.MapFrom(ps => ps.Product.Title)).ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDisplayDto>().ForMember(pts => pts.ProductName, opt => opt.MapFrom(ps => ps.Product.Title)).ReverseMap();
        }
    }
}
