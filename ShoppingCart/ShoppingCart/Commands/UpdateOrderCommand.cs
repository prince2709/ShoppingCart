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
    public class UpdateOrderCommand : IRequest<(int?, Response)>
    {
        public int OrderID { get; set; }
        public virtual IList<OrderItem> OrderItems { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public int CustomerID { get; set; }
        public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, (int?, Response)>
        {
            private readonly IOrderRepo _iOrderRepo;
            private readonly ICustomerRepo _iCustomerRepo;
            public UpdateOrderCommandHandler(IOrderRepo iOrderRepo, ICustomerRepo iCustomerRepo)
            {
                _iOrderRepo = iOrderRepo;
                _iCustomerRepo = iCustomerRepo;
            }

            public async Task<(int?, Response)> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
            {
                var orderDetails = await _iOrderRepo.GetOrderByIDAsync(command.OrderID);

                if (orderDetails == null)
                {
                    return (null, new Response(State.notFound, false, "Order item not found"));
                }
                else if (command.CustomerID <= 0 || command.ExpectedDeliveryDate == null)
                {
                    return (null, new Response(State.badRequest, false, "Order Details are not filled!"));
                }
                else if (await _iCustomerRepo.GetCustomerDetailsByIDAsync(command.CustomerID) == null)
                {
                    return (null, new Response(State.badRequest, false, "Invalid customer details!"));
                }
                else
                {
                    orderDetails.ExpectedDeliveryDate = command.ExpectedDeliveryDate;
                    orderDetails.ActualDeliveryDate = command.ActualDeliveryDate;
                    orderDetails.CustomerID = command.CustomerID;
                    int? orderID = await _iOrderRepo.UpdateOrderAsync(command.OrderID, orderDetails);
                    return (orderID, new Response(State.ok, true, null));
                }
            }
        }
    }
}
