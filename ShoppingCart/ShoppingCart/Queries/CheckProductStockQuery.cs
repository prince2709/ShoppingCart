using MediatR;
using ShoppingCart.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Queries
{
    public class CheckProductStockQuery : IRequest<bool>
    {
        public int ProductID { get; set; }
        public int RequiredCount { get; set; }
        public class CheckProductStockQueryHandler : IRequestHandler<CheckProductStockQuery, bool>
        {
            private readonly IProductRepo _iProductRepo;

            public CheckProductStockQueryHandler(IProductRepo iProductRepo)
            {
                _iProductRepo = iProductRepo;
            }

            public async Task<bool> Handle(CheckProductStockQuery query, CancellationToken cancellationToken)
            {
                return await _iProductRepo.CheckProductStockCount(query.ProductID, query.RequiredCount);
            }
        }
    }
}
