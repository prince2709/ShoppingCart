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
    public class GetCustomerDetailsByCustomerIDQuery: IRequest<Customer>
    {
        public int CustomerID { get; set; }
        public class GetCustomerDetailsByCustomerIDQueryHandler : IRequestHandler<GetCustomerDetailsByCustomerIDQuery, Customer>
        {
            private readonly ICustomerRepo _iCustomerRepo;

            public GetCustomerDetailsByCustomerIDQueryHandler(ICustomerRepo iCustomerRepo)
            {
                _iCustomerRepo = iCustomerRepo;
            }
            public async Task<Customer> Handle(GetCustomerDetailsByCustomerIDQuery query, CancellationToken cancellationToken)
            {
                return await _iCustomerRepo.GetCustomerDetailsByIDAsync(query.CustomerID);
            }

        }
    }
}
