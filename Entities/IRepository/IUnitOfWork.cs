﻿namespace Entities.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IJobRepository Job { get; }
        ICategoryRepository Category { get; }
        IProductRepository Product { get; }
        ICustomerRepository Customer { get; }
        ICustomerPaymentRepository CustomerPayment { get; }
        IExpenseRepository Expense { get; }
        IExpenseTypeRepository ExpenseType { get; }
        ILoanRepository Loan { get; }
        IOrderHeaderRepository OrderHeader { get; }
        IOrderDetailRepository OrderDetail { get; }
        IShoppingCartRepository ShoppingCart { get; }
        IApplicationUserRepository ApplicationUser { get; }
        ISettingRepository Setting { get; }
        IMoneySafeRepository MoneySafe { get; }
        IRefreshTokenRepository RefreshToken { get; } 
        Task<int> Complete();
    }
}
