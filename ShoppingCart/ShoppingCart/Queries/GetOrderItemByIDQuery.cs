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
    public class GetOrderItemByIDQuery : IRequest<OrderItem>
    {
        public int OrderItemID { get; set; }
        public class GetOrderItemByIDQueryHandler : IRequestHandler<GetOrderItemByIDQuery, OrderItem>
        {
            private readonly IOrderItemRepo _iOrderItemRepo;

            public GetOrderItemByIDQueryHandler(IOrderItemRepo iOrderItemRepo)
            {
                _iOrderItemRepo = iOrderItemRepo;
            }
            public async Task<OrderItem> Handle(GetOrderItemByIDQuery query, CancellationToken cancellationToken)
            {
                return await _iOrderItemRepo.GetOrderItemByIDAsync(query.OrderItemID);
            }
        }
    }
}
