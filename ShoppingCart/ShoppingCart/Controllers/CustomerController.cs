using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Commands;
using ShoppingCart.Contracts;
using ShoppingCart.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IMediator _mediator;
        public CustomerController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetCustomerDetailsByOrderIDAsync/{orderID:int}")]
        public async Task<IActionResult> GetCustomerDetailsByOrderIDAsync([FromRoute] int orderID)
        {
            var customerDetails = await _mediator.Send(new GetCustomerDetailsByOrderIDQuery { OrderID = orderID });
            if (customerDetails == null)
            {
                return NotFound();
            }
            return Ok(customerDetails);
        }

        [HttpGet]
        [Route("GetCustomerDetailsByIDAsync/{customerID:int}")]
        public async Task<IActionResult> GetCustomerDetailsByIDAsync([FromRoute] int customerID)
        {
            var customerDetails = await _mediator.Send(new GetCustomerDetailsByCustomerIDQuery { CustomerID = customerID });
            if (customerDetails == null)
            {
                return NotFound();
            }

            return Ok(customerDetails);

        }

        [HttpPut]
        [Route("{customerID:int}")]
        public async Task<IActionResult> UpdateCustomerDetailsAsync([FromRoute] int customerID, [FromBody] UpdateCustomerCommand command)
        {
            if (customerID != command.CustomerID)
            {
                return BadRequest();
            }
            else
            {
                var updateCustomerDetailStatus = await _mediator.Send(command);
                if (updateCustomerDetailStatus.Item2.IsSuccessful == false)
                {
                    return NotFound(updateCustomerDetailStatus.Item2.ResponseMessage);
                }
                return Ok(updateCustomerDetailStatus.Item1);
            }
        }
    }
}
