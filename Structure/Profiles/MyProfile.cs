using AutoMapper;
using Entities.DTO;
using Entities.IRepository;
using Entities.Models;

namespace Structure.Profiles
{
    public class MyProfile : Profile
    {
        private readonly IUnitOfWork _unitOfWork;
        public MyProfile(/*IUnitOfWork unitOfWork*/)
        {
            //_unitOfWork = unitOfWork;

            //Create
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Job, JobCreateDto>().ReverseMap();
            CreateMap<Category, CategoryCreateDto>().ReverseMap();
            CreateMap<Customer, CustomerCreateDto>().ReverseMap();
            CreateMap<CustomerPayment, CustomerPaymentCreateDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartCreateDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderCreateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailCreateDto>().ReverseMap();
            CreateMap<Loan, LoanCreateDto>().ReverseMap();
            CreateMap<Expense, ExpenseCreateDto>().ReverseMap();
            CreateMap<MoneySafe, MoneySafeCreateDto>().ReverseMap();

            //Update
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Category, CategoryUpdateDto>().ReverseMap();
            CreateMap<Job, JobUpdateDto>().ReverseMap();
            CreateMap<Customer, CustomerUpdateDto>().ReverseMap();
            CreateMap<CustomerPayment, CustomerPaymentUpdateDto>().ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartUpdateDto>().ReverseMap();
            CreateMap<OrderHeader, OrderHeaderUpdateDto>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailUpdateDto>().ReverseMap();
            CreateMap<Loan, LoanUpdateDto>().ReverseMap();
            CreateMap<Expense, ExpenseUpdateDto>().ReverseMap();
            CreateMap<MoneySafe, MoneySafeUpdateDto>().ReverseMap();

            //Display
            CreateMap<Product, ProductDisplayDto>()
                .ForMember(pts => pts.CategoryName, opt => opt.MapFrom(ps => ps.Category.Name))
                .ReverseMap();
            CreateMap<Category, CategoryDisplayDto>()
                .ReverseMap(); 
            CreateMap<Job, JobDisplayDto>()
                .ReverseMap();
            CreateMap<Customer, CustomerDisplayDto>()
                .ForMember(pts => pts.AddedUserName, opt => opt.MapFrom(ps => ps.ApplicationUser.UserName))
                .ReverseMap();
            CreateMap<CustomerPayment, CustomerPaymentDisplayDto>()
                .ForMember(pts => pts.MoneySafeName, opt => opt.MapFrom(ps => ps.MoneySafe.Name))
                .ForMember(pts => pts.AddedUserName, opt => opt.MapFrom(ps => ps.ApplicationUser.UserName))
                .ReverseMap();
            CreateMap<ApplicationUser, DisplayUserDto>()
                .ForMember(pts => pts.JobName, opt => opt.MapFrom(ps => ps.Job.Name))
                .ForMember(pts => pts.Phone, opt => opt.MapFrom(ps => ps.PhoneNumber))
                .ReverseMap();
            CreateMap<OrderHeader, OrderHeaderDisplayDto>()
                .ForMember(pts => pts.CustomerName, opt => opt.MapFrom(ps => ps.Customer.CustomerName))
                .ForMember(pts => pts.TechName, opt => opt.MapFrom(ps => ps.Tech.Name))
                .ForMember(pts => pts.ApplicationUserName, opt => opt.MapFrom(ps => ps.ApplicationUser.Name))
                .ReverseMap();
            CreateMap<OrderDetail, OrderDetailDisplayDto>()
                .ForMember(pts => pts.ProductName, opt => opt.MapFrom(ps => ps.Product.Title))
                .ReverseMap();
            CreateMap<ShoppingCart, ShoppingCartDisplayDto>()
                .ForMember(pts => pts.ProductName, opt => opt.MapFrom(ps => ps.Product.Title))
                .ReverseMap();
            CreateMap<MoneySafe, MoneySafeDisplayDto>()
                .ForMember(pts => pts.ApplicationUser, opt => opt.MapFrom(ps => ps.ApplicationUser.Name))
                .ForMember(pts => pts.ApplicationUserId, opt => opt.MapFrom(ps => ps.ApplicationUserId))
                //.ForMember(pts => pts.CurrentBalance, opt => opt.MapFrom(ps => _unitOfWork.MoneySafe.GetCurrentBalance(ps.Id)))
                .ReverseMap();
            CreateMap<Loan, LoanDisplayDto>()
                .ForMember(pts => pts.MoneySafe, opt => opt.MapFrom(ps => ps.MoneySafe.Name))
                .ForMember(pts => pts.ApplicationUser, opt => opt.MapFrom(ps => ps.ApplicationUser.Name))
                .ForMember(pts => pts.Emp, opt => opt.MapFrom(ps => ps.Emp.Name))
                .ReverseMap();
            CreateMap<Expense, ExpenseDisplayDto>()
                .ForMember(pts => pts.ExpenseType, opt => opt.MapFrom(ps => ps.ExpenseType.Name))
                .ForMember(pts => pts.Emp, opt => opt.MapFrom(ps => ps.Emp.Name))
                .ForMember(pts => pts.ApplicationUser, opt => opt.MapFrom(ps => ps.ApplicationUser.Name))
                .ForMember(pts => pts.MoneySafe, opt => opt.MapFrom(ps => ps.MoneySafe.Name))
                .ReverseMap();
        }
    }
}
