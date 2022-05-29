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
    public class AddNewOrderCommand : IRequest<(int?, Response)>
    {
        public virtual IList<OrderItem> OrderItems { get; set; }
        public DateTime ExpectedDeliveryDate { get; set; }
        public int CustomerID { get; set; }

        public class AddNewOrderCommandHandler : IRequestHandler<AddNewOrderCommand, (int?, Response)>
        {
            private readonly IOrderRepo _iOrderRepo;
            private readonly ICustomerRepo _iCustomerRepo;
            public AddNewOrderCommandHandler(IOrderRepo iOrderRepo, ICustomerRepo iCustomerRepo)
            {
                _iOrderRepo = iOrderRepo;
                _iCustomerRepo = iCustomerRepo;
            }

            public async Task<(int?, Response)> Handle(AddNewOrderCommand command, CancellationToken cancellationToken)
            {
                int? orderID = null;
                if (command.CustomerID <= 0 || command.OrderItems.Count <= 0 || command.ExpectedDeliveryDate == null)
                {
                    return (null, new Response(State.badRequest, false, "Order Details are not filled!"));
                }
                else if (await _iCustomerRepo.GetCustomerDetailsByIDAsync(command.CustomerID) == null)
                {
                    return (null, new Response(State.badRequest, false, "Invalid customer details!"));
                }
                else
                {
                    Order order = new Order();
                    order.OrderDate = DateTime.Now;
                    order.ExpectedDeliveryDate = command.ExpectedDeliveryDate;
                    order.CustomerID = command.CustomerID;
                    orderID = await _iOrderRepo.AddNewOrderAsync(order);
                    return (orderID, new Response(State.ok, true, null));
                }
            }
        }
    }
}
