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
    public class GetOrderByIDQuery : IRequest<Order>
    {
        public int OrderID { get; set; }
        public class GetOrderByIDQueryHandler : IRequestHandler<GetOrderByIDQuery, Order>
        {
            private readonly IOrderRepo _iOrderRepo;

            public GetOrderByIDQueryHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }
            public async Task<Order> Handle(GetOrderByIDQuery query, CancellationToken cancellationToken)
            {
                return await _iOrderRepo.GetOrderByIDAsync(query.OrderID);
            }
        }
    }
}
