using MediatR;
using ShoppingCart.Contracts;
using ShoppingCart.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Commands
{
    public class UpdateProductCommand : IRequest<(int?, Response)>
    {
        public int ProductID { get; set; }
        public int TotalStockCount { get; set; }
        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, (int?, Response)>
        {
            private readonly IProductRepo _iProductRepo;
            public UpdateProductCommandHandler(IProductRepo iProductRepo)
            {
                _iProductRepo = iProductRepo;
            }
            public async Task<(int?, Response)> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var productDetails = _iProductRepo.GetProductByID(command.ProductID).Result;
                if (productDetails == null)
                {
                    return (null, new Response(State.notFound, false, "Product not found"));
                }
                else if (command.TotalStockCount < 0)
                {
                    return (null, new Response(State.badRequest, false, "Product stock count cannot be less than 0!"));
                }
                else
                {
                    productDetails.TotalStockCount = command.TotalStockCount;
                    await _iProductRepo.UpdateProduct(command.ProductID, productDetails);
                    return (productDetails.ProductID, new Response(State.ok, true, null));
                }
            }
        }
    }
}
