using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Contracts
{
    public interface IProductRepo
    {
        Task<Product> GetProductByID(int productID);
        Task<List<Product>> GetProductByOrderID(int orderID);
        Task<bool> CheckProductStockCount(int productID, int requiredCount);
        Task<int?> UpdateProduct(int productID, Product product);
    }
}
