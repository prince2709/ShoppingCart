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
    public class GetProductDetailsByIDQuery : IRequest<Product>
    {
        public int ProductID { get; set; }
        public class GetProductDetailsByIDQueryHandler : IRequestHandler<GetProductDetailsByIDQuery, Product>
        {
            private readonly IProductRepo _iProductRepo;

            public GetProductDetailsByIDQueryHandler(IProductRepo iProductRepo)
            {
                _iProductRepo = iProductRepo;
            }

            public async Task<Product> Handle(GetProductDetailsByIDQuery query, CancellationToken cancellationToken)
            {
                return await _iProductRepo.GetProductByID(query.ProductID);
            }
        }
    }
}
