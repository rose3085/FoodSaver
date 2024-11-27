using Application.DTO.Foods;
using Application.DTO.Payment;
using Application.Interfaces.Payment;
using Application.Response.Payment;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.Payment
{
    public class PaymentReturnController : Controller
    {
        private readonly IOrderService _orderService;
        public PaymentReturnController(IOrderService orderService)
        {
            _orderService = orderService;   
        }


        public IActionResult ReturnUrl([FromQuery] PaymentReturnUrlResponse model)
        {
           var status = model.Status;
            if (status == "Completed")
            {
                var orderRequest = new CreateOrderDto
                {
                    ProductId = model.purchase_order_id,
                };
                var requestModel = new PaymentRequestDto
                { 
                    PidX = model.Pidx,
                    Amount = model.Amount,
                };
                var request = _orderService.CreateOrder(orderRequest,requestModel);
                return View(request);
            }
            return View();
        }
    }
}
