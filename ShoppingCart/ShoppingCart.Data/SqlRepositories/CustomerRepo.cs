using ShoppingCart.Contracts;
using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ShoppingCart.Data.SqlRepositories
{
    public class CustomerRepo : ICustomerRepo
    {
        private readonly ShoppingCartDbContext _context;
        public CustomerRepo(ShoppingCartDbContext context)
        {
            this._context = context;
        }

        public async Task<Customer> GetCustomerDetailsByOrderIDAsync(int orderID)
        {
            return await _context.Customer
                .Join(_context.Order, c => c.CustomerID, o => o.CustomerID, (c, o) => new { cust = c, ord = o })
                .Where(o => o.ord.OrderID == orderID)
                .Select(res => res.cust).FirstOrDefaultAsync<Customer>();

        }

        public async Task<Customer> GetCustomerDetailsByIDAsync(int customerID)
        {
            return await _context.Customer
                .Where(c => c.CustomerID == customerID)
                .FirstOrDefaultAsync<Customer>();

        }
        public async Task<int?> UpdateCustomerDetailsAsync(int customerID, Customer customer)
        {
            var existingCustomer = _context.Customer.Where(t => t.CustomerID == customerID).FirstOrDefault();
            if (existingCustomer != null)
            {
                existingCustomer.Address1 = customer.Address1;
                existingCustomer.Address2 = customer.Address2;
                existingCustomer.Street = customer.Street;
                existingCustomer.Town = customer.Town;
                existingCustomer.PostCode = customer.PostCode;
                await _context.SaveChangesAsync();
                return existingCustomer.CustomerID;
            }
            return null;
        }
    }
}
