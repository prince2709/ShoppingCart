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
    public class UpdateCustomerCommand : IRequest<(int?, Response)>
    {
        public int CustomerID { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string PostCode { get; set; }
        public class UpdateCustomerCommandHandler : IRequestHandler<UpdateCustomerCommand, (int?, Response)>
        {
            private readonly ICustomerRepo _iCustomerRepo;
            public UpdateCustomerCommandHandler(ICustomerRepo iCustomerRepo)
            {
                _iCustomerRepo = iCustomerRepo;
            }
            public async Task<(int?, Response)> Handle(UpdateCustomerCommand command, CancellationToken cancellationToken)
            {
                var customerDetails = _iCustomerRepo.GetCustomerDetailsByIDAsync(command.CustomerID).Result;
                if (customerDetails == null)
                {
                    return (null, new Response(State.notFound, false, "Customer not found"));
                }
                else if (string.IsNullOrEmpty(command.Address1) || string.IsNullOrEmpty(command.Address2) ||
                    string.IsNullOrEmpty(command.Street) || string.IsNullOrEmpty(command.Town) ||
                    string.IsNullOrEmpty(command.PostCode))
                {
                    return (null, new Response(State.badRequest, false, "Customer Details are not filled!"));
                }
                else
                {
                    customerDetails.Address1 = command.Address1;
                    customerDetails.Address2 = command.Address2;
                    customerDetails.Street = command.Street;
                    customerDetails.Town = command.Town;
                    customerDetails.PostCode = command.PostCode;
                    await _iCustomerRepo.UpdateCustomerDetailsAsync(command.CustomerID, customerDetails);
                    return (customerDetails.CustomerID, new Response(State.ok, true, null));
                }
            }
        }
    }
}
