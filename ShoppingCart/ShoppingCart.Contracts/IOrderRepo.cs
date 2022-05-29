using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Contracts
{
    public interface IOrderRepo
    {
        Task<List<Order>> GetAllOrdersAsync(int pageNo, int pageSize);
        Task<Order> GetOrderByIDAsync(int orderID);
        Task<int?> AddNewOrderAsync(Order order);
        Task<int?> UpdateOrderAsync(int orderID, Order order);
        Task<int?> DeleteOrderAsync(int orderID);
    }
}
