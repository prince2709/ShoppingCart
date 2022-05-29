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
    public class GetAllOrdersQuery : IRequest<List<Order>>
    {
        public int PageNo { get; set; }
        public int PageSize { get; set; }

        public class GetAllOrdersQueryHandler : IRequestHandler<GetAllOrdersQuery, List<Order>>
        {
            private readonly IOrderRepo _iOrderRepo;

            public GetAllOrdersQueryHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }
            public async Task<List<Order>> Handle(GetAllOrdersQuery query, CancellationToken cancellationToken)
            {
                return await _iOrderRepo.GetAllOrdersAsync(query.PageNo, query.PageSize);
            }
        }
    }
}
