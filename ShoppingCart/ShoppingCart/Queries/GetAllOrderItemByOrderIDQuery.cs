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
    public class GetAllOrderItemByOrderIDQuery : IRequest<List<OrderItem>>
    {
        public int OrderID { get; set; }
        public class GetAllOrderItemByOrderIDQueryHandler : IRequestHandler<GetAllOrderItemByOrderIDQuery, List<OrderItem>>
        {
            private readonly IOrderItemRepo _iOrderItemRepo;

            public GetAllOrderItemByOrderIDQueryHandler(IOrderItemRepo iOrderItemRepo)
            {
                _iOrderItemRepo = iOrderItemRepo;
            }
            public async Task<List<OrderItem>> Handle(GetAllOrderItemByOrderIDQuery query, CancellationToken cancellationToken)
            {
                return await _iOrderItemRepo.GetAllOrderItemByOrderIDsAsync(query.OrderID);
            }
        }
    }
}
