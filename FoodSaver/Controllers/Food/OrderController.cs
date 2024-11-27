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
        [Route("CreateOrder")]
        [Authorize]
        public async Task<IActionResult> PlaceOrder(CreateOrderDto createOrderRequest,PaymentRequestDto paymentRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.CreateOrder(createOrderRequest,paymentRequest);
                return Ok(result);

            }
            return BadRequest();
        }
    }
}
