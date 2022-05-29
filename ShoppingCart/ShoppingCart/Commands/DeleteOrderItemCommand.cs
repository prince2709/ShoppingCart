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
    public class DeleteOrderItemCommand : IRequest<(int?, Response)>
    {
        public int OrderItemID { get; set; }
        public class DeleteOrderItemCommandHandler : IRequestHandler<DeleteOrderItemCommand, (int?, Response)>
        {
            private readonly IOrderItemRepo _iOrderItemRepo;
            private readonly IProductRepo _iProductRepo;
            public DeleteOrderItemCommandHandler(IOrderItemRepo iOrderItemRepo, IProductRepo iProductRepo)
            {
                _iOrderItemRepo = iOrderItemRepo;
                _iProductRepo = iProductRepo;
            }
            public async Task<(int?, Response)> Handle(DeleteOrderItemCommand command, CancellationToken cancellationToken)
            {
                var orderItemDetails = _iOrderItemRepo.GetOrderItemByIDAsync(command.OrderItemID).Result;

                if (orderItemDetails == null)
                {
                    return (null, new Response(State.notFound, false, "Order item not found"));
                }
                else if (orderItemDetails.Order.ActualDeliveryDate != null)
                {
                    return (null, new Response(State.badRequest, false, "Order has been delivered. Cannot be deleted!"));
                }
                else
                {
                    int? orderItemID = await _iOrderItemRepo.DeleteOrderItemAsync(command.OrderItemID);
                    Product product = new Product();
                    product = await _iProductRepo.GetProductByID(orderItemDetails.ProductID);
                    product.TotalStockCount = (product.TotalStockCount + orderItemDetails.ProductQuantity);
                    await _iProductRepo.UpdateProduct(product.ProductID, product);
                    return (orderItemID, new Response(State.ok, true, null));
                }
            }
        }
    }
}
