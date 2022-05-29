using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Commands;
using ShoppingCart.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderItemController : ControllerBase
    {
        private IMediator _mediator;
        public OrderItemController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetOrderItemsByIDAsync/{orderItemID:int}")]
        public async Task<IActionResult> GetOrderItemsByIDAsync([FromRoute] int orderItemID)
        {
            var orderItem = await _mediator.Send(new GetOrderItemByIDQuery { OrderItemID = orderItemID });
            if (orderItem == null)
            {
                return NotFound();
            }
            return Ok(orderItem);
        }

        [HttpGet]
        [Route("GetAllOrderItemsByOrderIDAsync/{orderID:int}")]
        public async Task<IActionResult> GetAllOrderItemsByOrderIDAsync([FromRoute] int orderID)
        {
            var orderItemList = await _mediator.Send(new GetAllOrderItemByOrderIDQuery { OrderID = orderID });
            if (orderItemList == null)
            {
                return NotFound();
            }
            return Ok(orderItemList);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewOrderItemAsync([FromBody] AddNewOrderItemCommand command)
        {
            var addedOrderItemStatus = await _mediator.Send(new AddNewOrderItemCommand
            {
                OrderID = command.OrderID,
                ProductID = command.ProductID,
                ProductQuantity = command.ProductQuantity
            });
            if (addedOrderItemStatus.Item2.IsSuccessful == false)
            {
                return NotFound(addedOrderItemStatus.Item2.ResponseMessage);
            }
            return Ok(addedOrderItemStatus.Item1);
        }

        [HttpPut]
        [Route("{orderItemID:int}")]
        public async Task<IActionResult> UpdateOrderItemAsync([FromRoute] int orderItemID, [FromBody] UpdateOrderItemCommand command)
        {
            if (orderItemID != command.OrderItemID)
            {
                return BadRequest();
            }
            else
            {
                var updateOrderItemStatus = await _mediator.Send(command);
                if (updateOrderItemStatus.Item2.IsSuccessful == false)
                {
                    return NotFound(updateOrderItemStatus.Item2.ResponseMessage);
                }
                return Ok(updateOrderItemStatus.Item1);
            }
        }

        [HttpDelete]
        [Route("{orderItemID:int}")]
        public async Task<IActionResult> DeleteOrderItemAsync([FromRoute] int orderItemID)
        {
            var deletedOrderItemStatus = await _mediator.Send(new DeleteOrderItemCommand { OrderItemID = orderItemID });
            if (deletedOrderItemStatus.Item2.IsSuccessful == false)
            {
                return NotFound(deletedOrderItemStatus.Item2.ResponseMessage);
            }
            return Ok(deletedOrderItemStatus.Item1);
        }
    }
}
