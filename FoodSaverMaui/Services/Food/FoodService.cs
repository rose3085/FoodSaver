
using FoodSaverMaui.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.Food
{
    public class FoodService
    {
        private readonly HttpClient _httpClient;

        public FoodService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<GetProductsResponse>> GetAllProducts()
        {
            try
            {

                var url = $"{App.Settings.ApiBaseUrl}/api/Food/GetFood";
                var response = await _httpClient.GetAsync(url, CancellationToken.None);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {

                    var result = await response.Content.ReadFromJsonAsync<IEnumerable<GetProductsResponse>>();
                    return result;

                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
