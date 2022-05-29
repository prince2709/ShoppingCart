using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Commands;
using ShoppingCart.Entities;
using ShoppingCart.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IMediator _mediator;
        public OrderController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("GetAllOrders/{pageNo:int}/{pageSize:int}")]
        public async Task<IActionResult> GetAllOrdersAsync(int pageNo, int pageSize)
        {
            var orderList = await _mediator.Send(new GetAllOrdersQuery { PageNo = pageNo, PageSize = pageSize });
            if (orderList == null)
            {
                return NotFound();
            }
            return Ok(orderList);
        }

        [HttpGet]
        [Route("GetOrderByIDAsync/{orderID:int}")]
        public async Task<IActionResult> GetOrderByIDAsync(int orderID)
        {
            var orderDetails = await _mediator.Send(new GetOrderByIDQuery { OrderID = orderID });
            if (orderDetails == null)
            {
                return NotFound();
            }
            return Ok(orderDetails);
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> AddNewOrderAsync([FromBody] AddNewOrderCommand command)
        {
            var addedOrderStatus = await _mediator.Send(new AddNewOrderCommand
            {
                OrderItems = command.OrderItems,
                ExpectedDeliveryDate = command.ExpectedDeliveryDate,
                CustomerID = command.CustomerID
            });
            if (addedOrderStatus.Item2.IsSuccessful == false)
            {
                return NotFound(addedOrderStatus.Item2.ResponseMessage);
            }
            else
            {
                foreach (OrderItem orderItem in command.OrderItems)
                {
                    var addedOrderItemStatus = await _mediator.Send(new AddNewOrderItemCommand
                    {
                        OrderID = addedOrderStatus.Item1.Value,
                        ProductID = orderItem.ProductID,
                        ProductQuantity = orderItem.ProductQuantity
                    });
                    if (addedOrderItemStatus.Item2.IsSuccessful == false)
                    {
                        return NotFound(addedOrderItemStatus.Item2.ResponseMessage);
                    }
                }
            }
            return Ok(addedOrderStatus.Item1);
        }

        [HttpPut]
        [Route("{orderID:int}")]
        public async Task<IActionResult> UpdateOrderAsync([FromRoute] int orderID, [FromBody] UpdateOrderCommand command)
        {
            if (orderID != command.OrderID)
            {
                return BadRequest();
            }
            else
            {
                var updateOrderStatus = await _mediator.Send(command);
                if (updateOrderStatus.Item2.IsSuccessful == false)
                {
                    return NotFound(updateOrderStatus.Item2.ResponseMessage);
                }
                else if (command.OrderItems.Count > 0)
                {
                    foreach (OrderItem orderItem in command.OrderItems)
                    {
                        if (orderItem.OrderItemID == 0)
                        {
                            var addedOrderItemStatus = await _mediator.Send(new AddNewOrderItemCommand
                            {
                                OrderID = updateOrderStatus.Item1.Value,
                                ProductID = orderItem.ProductID,
                                ProductQuantity = orderItem.ProductQuantity
                            });
                            if (addedOrderItemStatus.Item2.IsSuccessful == false)
                            {
                                return NotFound(addedOrderItemStatus.Item2.ResponseMessage);
                            }
                        }
                        else if (orderItem.OrderID != orderID)
                        {
                            return BadRequest("Order items doesn't belongs to this order");
                        }
                        else
                        {
                            var addedOrderItemStatus = await _mediator.Send(new UpdateOrderItemCommand
                            {
                                OrderItemID = orderItem.OrderItemID,
                                ProductQuantity = orderItem.ProductQuantity
                            });
                            if (addedOrderItemStatus.Item2.IsSuccessful == false)
                            {
                                return NotFound(addedOrderItemStatus.Item2.ResponseMessage);
                            }
                        }
                    }
                }
                return Ok(updateOrderStatus.Item1);
            }
        }

        [HttpDelete]
        [Route("{orderID:int}")]
        public async Task<IActionResult> DeleteOrderAsync([FromRoute] int orderID)
        {
            var orderItemList = await _mediator.Send(new GetAllOrderItemByOrderIDQuery { OrderID = orderID });
            if (orderItemList != null && orderItemList.Count > 0)
            {
                foreach (OrderItem ordertItem in orderItemList)
                {
                    var deletedOrderItemStatus = await _mediator.Send(new DeleteOrderItemCommand { OrderItemID = ordertItem.OrderItemID });
                    if (deletedOrderItemStatus.Item2.IsSuccessful == false)
                    {
                        return NotFound(deletedOrderItemStatus.Item2.ResponseMessage);
                    }
                }
            }

            var deletedOrderStatus = await _mediator.Send(new DeleteOrderCommand { OrderID = orderID });
            if (deletedOrderStatus.Item2.IsSuccessful == false)
            {
                return NotFound(deletedOrderStatus.Item2.ResponseMessage);
            }
            return Ok(deletedOrderStatus.Item1);
        }
    }
}
