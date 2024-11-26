using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.Payment
{
    public class PaymentReturnController : Controller
    {
      

        public ActionResult ReturnUrl()
        {
            return View();
        }
    }
}
