using MediatR;
using ShoppingCart.Contracts;
using ShoppingCart.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Queries
{
    public class GetCustomerDetailsByOrderIDQuery : IRequest<Customer>
    {
        public int OrderID { get; set; }
        public class GetCustomerDetailsByOrderIDQueryHandler : IRequestHandler<GetCustomerDetailsByOrderIDQuery, Customer>
        {
            private readonly ICustomerRepo _iCustomerRepo;

            public GetCustomerDetailsByOrderIDQueryHandler(ICustomerRepo iCustomerRepo)
            {
                _iCustomerRepo = iCustomerRepo;
            }
            public async Task<Customer> Handle(GetCustomerDetailsByOrderIDQuery query, CancellationToken cancellationToken)
            {
                return await _iCustomerRepo.GetCustomerDetailsByOrderIDAsync(query.OrderID);
            }
        }
    }
}
