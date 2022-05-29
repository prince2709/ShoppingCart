using MediatR;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingCart.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private IMediator _mediator;
        public ProductController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{productID:int}")]
        public async Task<IActionResult> GetProductByIDAsync([FromRoute] int productID)
        {
            var productDetails = await _mediator.Send(new GetProductDetailsByIDQuery { ProductID = productID });
            if (productDetails == null)
            {
                return NotFound();
            }
            return Ok(productDetails);
        }

        [HttpGet]
        [Route("GetProductByOrderIDAsync/{orderID:int}")]
        public async Task<IActionResult> GetProductByOrderIDAsync([FromRoute] int orderID)
        {
            var productList = await _mediator.Send(new GetProductDetailsByOrderIDQuery { OrderID = orderID });
            if (productList == null)
            {
                return NotFound();
            }
            return Ok(productList);
        }
    }
}
