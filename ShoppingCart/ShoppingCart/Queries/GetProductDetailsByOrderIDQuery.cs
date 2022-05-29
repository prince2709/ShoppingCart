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
    public class GetProductDetailsByOrderIDQuery : IRequest<List<Product>>
    {
        public int OrderID { get; set; }
        public class GetProductDetailsByIDQueryHandler : IRequestHandler<GetProductDetailsByOrderIDQuery, List<Product>>
        {
            private readonly IProductRepo _iProductRepo;

            public GetProductDetailsByIDQueryHandler(IProductRepo iProductRepo)
            {
                _iProductRepo = iProductRepo;
            }

            public async Task<List<Product>> Handle(GetProductDetailsByOrderIDQuery query, CancellationToken cancellationToken)
            {
                return await _iProductRepo.GetProductByOrderID(query.OrderID);
            }
        }
    }
}
