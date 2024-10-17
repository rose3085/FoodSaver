using Application.DTO.Foods;
using Application.DTO.User;
using Application.Interfaces.Food;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FoodSaver.Controllers.Food
{
    [Route("api/[controller]")]
    [ApiController]
    public class FoodController : ControllerBase
    {
        private readonly IFoodService _foodService;

        public FoodController(IFoodService foodService)
        {
            _foodService = foodService;
        }

        [HttpGet]
        [Route("GetFood")]
       
        public async Task<IActionResult> GetProducts()
        {
            var products = await _foodService.GetProductsAsync();
            return Ok(products);
        }

        [HttpPost]
        [Route("AddFood")]
        [Authorize]
        public async Task<IActionResult> AddFood(PostFoodRequestDto postFood)
        {
            if (ModelState.IsValid)
            {
                var result = await _foodService.AddFood(postFood);
                return Ok(result);

            }
            return BadRequest();
        }

        [HttpDelete]
        [Route("DeleteFood")]
        [Authorize]
        public async Task<IActionResult> DeleteFood(string id)
        {
            if (ModelState.IsValid)
            {
                var result = await _foodService.DeleteFood(id);
                return Ok(result);

            }
            return BadRequest();
        }
    }
}
