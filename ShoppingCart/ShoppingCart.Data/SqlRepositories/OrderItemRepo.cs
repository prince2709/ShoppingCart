using Microsoft.EntityFrameworkCore;
using ShoppingCart.Contracts;
using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.SqlRepositories
{
    public class OrderItemRepo : IOrderItemRepo
    {
        private readonly ShoppingCartDbContext _context;
        public OrderItemRepo(ShoppingCartDbContext context)
        {
            this._context = context;
        }

        public async Task<OrderItem> GetOrderItemByIDAsync(int orderItemID)
        {
            return await _context.OrderItem
               .Where(otm => otm.OrderItemID == orderItemID)
               .Include(product => product.Product)
                .Include(order => order.Order)
              .FirstOrDefaultAsync();
        }
        public async Task<List<OrderItem>> GetAllOrderItemByOrderIDsAsync(int orderID)
        {
            return await _context.OrderItem
               .Where(otm => otm.Order.OrderID == orderID)
               .Include(product => product.Product)
              .ToListAsync();
        }
        public async Task<int?> AddNewOrderItemAsync(OrderItem orderItem)
        {
            var orderItemSaved = await _context.OrderItem.AddAsync(orderItem);
            await _context.SaveChangesAsync();
            return orderItemSaved.Entity.OrderItemID;
        }
        public async Task<int?> UpdateOrderItemAsync(int orderItemID, OrderItem orderItem)
        {
            var existingOrderItem = _context.OrderItem.Where(t => t.OrderItemID == orderItemID).FirstOrDefault();
            if (existingOrderItem != null)
            {
                existingOrderItem.ProductQuantity = orderItem.ProductQuantity;
                await _context.SaveChangesAsync();
                return existingOrderItem.OrderItemID;
            }
            return null;
        }
        public async Task<int?> DeleteOrderItemAsync(int orderItemID)
        {
            var orderItemDetails = _context.OrderItem.Where(t => t.OrderItemID == orderItemID).FirstOrDefault();
            if (orderItemDetails != null)
            {
                _context.OrderItem.Remove(orderItemDetails);
                await _context.SaveChangesAsync();
                return orderItemDetails.OrderItemID;
            }
            return null;
        }
    }
}
