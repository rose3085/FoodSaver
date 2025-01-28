using FoodSaverMaui.Model;
using FoodSaverMaui.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace FoodSaverMaui.Services.OrderDelivery
{
   public class DeliveryService
    {
        private readonly HttpClient _httpClient;

        public DeliveryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<ResponseManager> UpdateDeliveryStatus(UpdateDeliveryStatusModel requestModel)
        {

            try
            {
                var jwtToken = await SecureStorage.GetAsync("token");
                if (jwtToken == null)
                {

                    return null;
                }

                var url = $"{App.Settings.ApiBaseUrl}/api/OrderDelivery/UpateDeliveryStatus";
                var json = JsonConvert.SerializeObject(requestModel);
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwtToken);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                Console.WriteLine($"s Code: {(int)response.StatusCode}");
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<ResponseManager>();
                    return result;
                }
                else
                {

                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;

            }
        }
    }
}
