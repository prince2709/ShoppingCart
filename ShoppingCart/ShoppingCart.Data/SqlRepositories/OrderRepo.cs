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
    public class OrderRepo : IOrderRepo
    {
        private readonly ShoppingCartDbContext _context;
        public OrderRepo(ShoppingCartDbContext context)
        {
            this._context = context;
        }
        public async Task<List<Order>> GetAllOrdersAsync(int pageNo, int pageSize)
        {
            return await _context.Order
                .Include(cust => cust.Customer)
                .Include(order => order.OrderItems)
                 .ThenInclude(p => p.Product)
                .Skip((pageNo - 1) * pageSize)
                        .Take(pageSize)
                .ToListAsync();
        }
        public async Task<Order> GetOrderByIDAsync(int orderID)
        {
            return await _context.Order.Where(t => t.OrderID == orderID)
                  .Include(cust => cust.Customer)
                .Include(order => order.OrderItems)
                 .ThenInclude(p => p.Product).FirstOrDefaultAsync();
        }
        public async Task<int?> AddNewOrderAsync(Order order)
        {
            var orderSaved = await _context.Order.AddAsync(order);
            await _context.SaveChangesAsync();
            return orderSaved.Entity.OrderID;

        }
        public async Task<int?> UpdateOrderAsync(int orderID, Order order)
        {
            var existingOrder = _context.Order.Where(t => t.OrderID == orderID).FirstOrDefault();
            if (existingOrder != null)
            {
                existingOrder.OrderDate = order.OrderDate;
                existingOrder.ExpectedDeliveryDate = order.ExpectedDeliveryDate;
                existingOrder.ActualDeliveryDate = order.ActualDeliveryDate;
                existingOrder.CustomerID = order.CustomerID;
                await _context.SaveChangesAsync();
                return existingOrder.OrderID;
            }
            return null;
        }
        public async Task<int?> DeleteOrderAsync(int orderID)
        {
            var orderDetails = _context.Order.Where(t => t.OrderID == orderID).FirstOrDefault();
            if (orderDetails != null)
            {
                _context.Order.Remove(orderDetails);
                await _context.SaveChangesAsync();
                return orderDetails.OrderID;
            }
            return null;
        }

    }
}
