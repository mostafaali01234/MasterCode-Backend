using DataAccess.Data;
using Entities.IRepository;
using Entities.Models;

namespace DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IOrderHeaderRepository OrderHeader { get; private set; }
        public IOrderDetailRepository OrderDetail { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductRepository Product { get; private set; }
        public ICustomerRepository Customer { get; private set; }
        public IShoppingCartRepository ShoppingCart { get; private set; }
        public IApplicationUserRepository ApplicationUser { get; private set; }
        public ISettingRepository Setting { get; private set; }
        public ICustomerPaymentRepository CustomerPayment { get; private set; }
        public IExpenseRepository Expense { get; private set; }
        public IExpenseTypeRepository ExpenseType { get; private set; }
        public ILoanRepository Loan { get; private set; }
        public IMoneySafeRepository MoneySafe { get; private set; }
        public IRefreshTokenRepository RefreshToken { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            OrderHeader = new OrderHeaderRepository(context);
            OrderDetail = new OrderDetailRepository(context);
            ShoppingCart = new ShoppingCartRepository(context);
            Category = new CategoryRepository(context);
            Product = new ProductRepository(context);
            Customer = new CustomerRepository(context);
            ApplicationUser = new ApplicationUserRepository(context);
            Setting = new SettingRepository(context);
            CustomerPayment = new CustomerPaymentRepository(context);
            Expense = new ExpenseRepository(context);
            ExpenseType = new ExpenseTypeRepository(context);
            Loan = new LoanRepository(context);
            MoneySafe = new MoneySafeRepository(context);
            RefreshToken = new RefreshTokenRepository(context);
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
