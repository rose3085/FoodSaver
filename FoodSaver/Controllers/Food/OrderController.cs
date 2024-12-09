using Application.DTO.Foods;
using Application.DTO.Payment;
using Application.Interfaces.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.Food
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;   
        }


        
       
       [HttpPost]
        
        public async Task<IActionResult> PlaceOrder([FromBody]CreateOrderDto createOrderRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.CreateOrder(createOrderRequest);
                return Ok(result);

            }
            return BadRequest();
        }
    }
}
