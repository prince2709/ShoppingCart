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
    public class DeleteOrderCommand : IRequest<(int?, Response)>
    {
        public int OrderID { get; set; }
        public class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, (int?, Response)>
        {
            private readonly IOrderRepo _iOrderRepo;
            public DeleteOrderCommandHandler(IOrderRepo iOrderRepo)
            {
                _iOrderRepo = iOrderRepo;
            }

            public async Task<(int?, Response)> Handle(DeleteOrderCommand command, CancellationToken cancellationToken)
            {
                var orderDetails = _iOrderRepo.GetOrderByIDAsync(command.OrderID).Result;

                if (orderDetails == null)
                {
                    return (null, new Response(State.notFound, false, "Order not found"));
                }
                else if (orderDetails.ActualDeliveryDate != null)
                {
                    return (null, new Response(State.badRequest, false, "Order has been delivered. Cannot be deleted!"));
                }
                else
                {
                    int? orderID = await _iOrderRepo.DeleteOrderAsync(command.OrderID);
                    return (orderID, new Response(State.ok, true, null));
                }
            }
        }
    }
}
