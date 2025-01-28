using Application.DTO.OrderDelivery;
using Application.Interfaces.OrderDelivery;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.OrderDelivery
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderDeliveryController : ControllerBase
    {
        private readonly IDeliveryService _deliveryService;

        public OrderDeliveryController(IDeliveryService deliveryService)
        {
            _deliveryService = deliveryService;
        }

        [HttpPost]
        [Route("UpateDeliveryStatus")]
        public async Task<IActionResult> UpdateDeliveryStatus([FromBody]UpdateDeliveryStatusDto requestDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _deliveryService.UpdateDeliveryStatus(requestDto);
                return Ok(result);
            
            }
            else { return BadRequest(); }
        
        }

    }
}
