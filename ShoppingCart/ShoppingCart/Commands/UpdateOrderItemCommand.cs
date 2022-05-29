using MediatR;
using ShoppingCart.Contracts;
using ShoppingCart.Entities;
using ShoppingCart.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ShoppingCart.Commands
{
    public class UpdateOrderItemCommand : IRequest<(int?, Response)>
    {
        public int OrderItemID { get; set; }
        public int ProductQuantity { get; set; }
        public class UpdateOrderItemCommandHandler : IRequestHandler<UpdateOrderItemCommand, (int?, Response)>
        {
            private readonly IOrderItemRepo _iOrderItemRepo;
            private readonly IProductRepo _iProductRepo;
            public UpdateOrderItemCommandHandler(IOrderItemRepo iOrderItemRepo, IProductRepo iProductRepo)
            {
                _iOrderItemRepo = iOrderItemRepo;
                _iProductRepo = iProductRepo;
            }
            public async Task<(int?, Response)> Handle(UpdateOrderItemCommand command, CancellationToken cancellationToken)
            {
                var orderItemDetails = await _iOrderItemRepo.GetOrderItemByIDAsync(command.OrderItemID);

                if (orderItemDetails == null)
                {
                    return (null, new Response(State.notFound, false, "Order item not found"));
                }
                else
                {
                    int extraProductCountReq = (command.ProductQuantity - orderItemDetails.ProductQuantity);
                    if (extraProductCountReq > 0 &&
                        (await _iProductRepo.CheckProductStockCount(orderItemDetails.ProductID, extraProductCountReq) == false))
                    {
                        return (null, new Response(State.badRequest, false, "Product is not available!"));
                    }
                    else
                    {
                        orderItemDetails.ProductQuantity = command.ProductQuantity;
                        int? orderItemID = await _iOrderItemRepo.UpdateOrderItemAsync(command.OrderItemID, orderItemDetails);
                        Product product = new Product();
                        product = await _iProductRepo.GetProductByID(orderItemDetails.ProductID);
                        product.TotalStockCount = (product.TotalStockCount - extraProductCountReq);
                        await _iProductRepo.UpdateProduct(product.ProductID, product);
                        return (orderItemID, new Response(State.ok, true, null));
                    }
                }
            }
        }
    }
}
