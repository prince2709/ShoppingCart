using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Contracts
{
    public interface ICustomerRepo
    {
        Task<Customer> GetCustomerDetailsByOrderIDAsync(int orderID);
        Task<Customer> GetCustomerDetailsByIDAsync(int customerID);
        Task<int?> UpdateCustomerDetailsAsync(int customerID, Customer customer);
    }
}
