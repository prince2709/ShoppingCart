using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Contracts
{
    public interface IOrderItemRepo
    {
        Task<OrderItem> GetOrderItemByIDAsync(int orderItemID);
        Task<List<OrderItem>> GetAllOrderItemByOrderIDsAsync(int orderID);
        Task<int?> AddNewOrderItemAsync(OrderItem orderItem);
        Task<int?> UpdateOrderItemAsync(int orderItemID, OrderItem orderItem);
        Task<int?> DeleteOrderItemAsync(int orderItemID);
    }
}
