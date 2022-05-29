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
    public class ProductRepo : IProductRepo
    {
        private readonly ShoppingCartDbContext _context;
        public ProductRepo(ShoppingCartDbContext context)
        {
            this._context = context;
        }
        public async Task<Product> GetProductByID(int productID)
        {
            return await _context.Product.Where(t => t.ProductID == productID)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Product>> GetProductByOrderID(int orderID)
        {
            return await _context.Product
                .Join(_context.OrderItem, p => p.ProductID, o => o.ProductID, (p, o) => new { prod = p, ordItm = o })
                .Where(o => o.ordItm.Order.OrderID == orderID)
                .Select(res => res.prod).ToListAsync<Product>();
        }
        public async Task<bool> CheckProductStockCount(int productID, int requiredCount)
        {
            var totalStockCount = await _context.Product
               .Where(p => p.ProductID == productID)
               .Select(t => t.TotalStockCount).FirstOrDefaultAsync();

            if (totalStockCount >= requiredCount)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<int?> UpdateProduct(int productID, Product product)
        {
            var existingProduct = _context.Product.Where(p => p.ProductID == productID).FirstOrDefault();
            if (existingProduct != null)
            {
                existingProduct.TotalStockCount = product.TotalStockCount;
                await _context.SaveChangesAsync();
                return existingProduct.ProductID;
            }
            return null;
        }
    }
}
