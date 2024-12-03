using Application.DTO.Foods;
using Application.Response.Food;
using Domain.Entities.Foods;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.Food
{
    public interface IFoodService
    {
        Task<FoodServiceResponse> AddFood(PostFoodRequestDto postFood);
        Task<string> SaveFileAsync(IFormFile imageFile, string[] allowedFileExtensions);

        Task<IEnumerable<GetProductResponse>> GetProductsAsync();

        Task<FoodServiceResponse> DeleteFood(string foodId);
        Task<IEnumerable<GetProductResponse>> GetUsersProduct();
    }
}
