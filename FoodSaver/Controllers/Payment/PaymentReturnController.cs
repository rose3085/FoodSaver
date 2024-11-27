using Application.DTO.Foods;
using Application.DTO.Payment;
using Application.Interfaces.Payment;
using Application.Response.Payment;
using FoodSaver.Controllers.Food;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.Payment
{
    public class PaymentReturnController : Controller
    {
        private readonly IOrderService _orderService;
        private readonly OrderController _orderController;

        public PaymentReturnController(IOrderService orderService,OrderController orderController)
        {
            _orderService = orderService;
            _orderController = orderController;
        }


        
         public async  Task<ActionResult> ReturnUrl([FromQuery] PaymentReturnUrlResponse model)
        {
           var status = model.Status;
            if (status == "Completed")
            {
                var orderRequest = new CreateOrderDto
                {
                    ProductId = model.purchase_order_id,
                    BuyerName = model.purchase_order_name,
                    PidX = model.Pidx,
                    Amount = model.Amount,
                };
                //var requestModel = new PaymentRequestDto
                //{ 
                //    PidX = model.Pidx,
                //    Amount = model.Amount,
                //};
                //var request = _orderController.PlaceOrder(orderRequest);


                using (var client = new HttpClient())
                {

                    var url = "https://localhost:7293/api/Order";
                    // Make the POST request to the PlaceOrder API
                    var response = await client.PostAsJsonAsync(url, orderRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        // Handle the response if needed (e.g., parsing response)
                        var result = await response.Content.ReadAsStringAsync();
                        
                    }
                    else
                    {
                        // Handle failed request
                        ModelState.AddModelError(string.Empty, "Error processing order.");
                    }
                }
            }




        
            return View();
        }

        

    }
}
