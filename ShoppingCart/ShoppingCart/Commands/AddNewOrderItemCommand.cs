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
    public class AddNewOrderItemCommand : IRequest<(int?, Response)>
    {
        public int ProductID { get; set; }
        public int ProductQuantity { get; set; }
        public int OrderID { get; set; }
        public class AddNewOrderItemCommandHandler : IRequestHandler<AddNewOrderItemCommand, (int?, Response)>
        {
            private readonly IOrderItemRepo _iOrderItemRepo;
            private readonly IProductRepo _iProductRepo;
            public AddNewOrderItemCommandHandler(IOrderItemRepo iOrderItemRepo, IProductRepo iProductRepo)
            {
                _iOrderItemRepo = iOrderItemRepo;
                _iProductRepo = iProductRepo;
            }

            public async Task<(int?, Response)> Handle(AddNewOrderItemCommand command, CancellationToken cancellationToken)
            {
                int? orderItemID = null;
                if (command.OrderID <= 0 || command.ProductID <= 0 || command.ProductQuantity <= 0)
                {
                    return (null, new Response(State.badRequest, false, "Order item Details are not filled!"));
                }
                else if (_iProductRepo.CheckProductStockCount(command.ProductID, command.ProductQuantity).Result == false)
                {
                    return (null, new Response(State.badRequest, false, "Product is not available!"));
                }
                else
                {
                    OrderItem orderItem = new OrderItem();
                    orderItem.OrderID = command.OrderID;
                    orderItem.ProductID = command.ProductID;
                    orderItem.ProductQuantity = command.ProductQuantity;
                    orderItemID = await _iOrderItemRepo.AddNewOrderItemAsync(orderItem);
                    Product product = new Product();
                    product = await _iProductRepo.GetProductByID(command.ProductID);
                    product.TotalStockCount = (product.TotalStockCount - command.ProductQuantity);
                    await _iProductRepo.UpdateProduct(command.ProductID, product);
                    return (orderItemID, new Response(State.ok, true, null));
                }
            }
        }
    }
}

