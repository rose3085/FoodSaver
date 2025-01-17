using Application.Interfaces.PurchaseHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.PurchaseHistory
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseHistoryController : ControllerBase
    {
        private readonly IPurchaseHistoryService _purchaseHistoryService;

        public PurchaseHistoryController(IPurchaseHistoryService purchaseHistoryService)
        {
            _purchaseHistoryService = purchaseHistoryService;
        }

        [Authorize]
        [HttpGet]
        [Route("GetUserPurchaseHistory")]
        public async Task<IActionResult> GetUserPurchaseHistory()
        {
            if (ModelState.IsValid)
            {
                var result = await _purchaseHistoryService.GetUserPurchasedFoods();
                return Ok(result);

            }
            return BadRequest();

        }
    }
}
