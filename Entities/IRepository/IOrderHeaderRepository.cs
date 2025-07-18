﻿using Entities.Models;

namespace Entities.IRepository
{
    public interface IOrderHeaderRepository : IGenericRepository<OrderHeader>
    {
        void Update(OrderHeader orderHeader);
        void CancelOrder(OrderHeader orderHeader);
        void CompleteOrder(OrderHeader orderHeader, string Techod);
    }
}
